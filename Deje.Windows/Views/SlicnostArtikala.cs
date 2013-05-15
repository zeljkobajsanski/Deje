using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Deje.AzureBlobStorage;
using Deje.Lucene;
using Deje.Repository.EF.Repository;
using Deje.Windows.Model;
using Shell;
using Shell.Events;

namespace Deje.Windows.Views
{
    public partial class SlicnostArtikala : ViewBase
    {
        private readonly KategorijeArtikalaRepository m_KategorijeArtikalaRepository = new KategorijeArtikalaRepository();

        private readonly ArtikliRepository m_ArtikliRepository = new ArtikliRepository();

        private readonly PictureStorageService m_StorageService = new PictureStorageService();

        private readonly Dictionary<int, byte[]> m_Slike = new Dictionary<int, byte[]>(); 

        private BindingList<SlicnostArtikla> m_Artikli = new BindingList<SlicnostArtikla>();

        private readonly Index m_Index = new Index();

        public SlicnostArtikala()
        {
            InitializeComponent();
            textEdit1.KeyDown += (s, e) =>
            {
                if (Keys.Enter == e.KeyCode) PretraziPoDeluNaziva();
            };
            btnPretrazi.Click += (s, e) => PretraziPoDeluNaziva();
        }

        protected override void OnLoad(EventArgs e)
        {
            OnRefresh();
        }

        public override void OnRefresh()
        {
            kategorijaArtiklaBindingSource.DataSource = m_KategorijeArtikalaRepository.VratiSve().OrderBy(x => x.Naziv);
            sinonimBindingSource.DataSource = m_ArtikliRepository.VratiSinonime().OrderBy(x => x.Naziv);
        }

        public override void OnSave()
        {
            gridView1.CloseEditor();
            var artikal = gridView1.GetFocusedRow() as SlicnostArtikla;
            if (artikal == null) return;
           
            if (artikal.SlikaRaw != null && artikal.SlikaRaw.Length > 0)
            {
                if (m_Slike.ContainsKey(artikal.Id))
                {
                    if (!m_Slike[artikal.Id].SequenceEqual(artikal.SlikaRaw))
                    {
                        artikal.Slika = m_StorageService.SacuvajSlikuArtikla(new MemoryStream(artikal.SlikaRaw), "image/jpeg", ".jpg");
                        m_Slike.Add(artikal.Id, artikal.SlikaRaw);
                    }
                } else
                {
                    artikal.Slika = m_StorageService.SacuvajSlikuArtikla(new MemoryStream(artikal.SlikaRaw), "image/jpeg", ".jpg");
                    m_Slike.Add(artikal.Id, artikal.SlikaRaw);
                }
            }

            m_ArtikliRepository.Sacuvaj(new Core.Model.Artikal
            {
                Id = artikal.Id,
                IdKategorijeArtikla = artikal.IdKategorijeArtikla,
                Naziv = artikal.Naziv,
                IdSinonima = artikal.IdSinonima,
                IdDobavljaca = artikal.IdDobavljaca,
                Slika = artikal.Slika,
                Opis = artikal.Opis,
                Cena = artikal.Cena,
                Aktivan = artikal.Aktivan
            });
        }

        private void Indeksiraj()
        {
            var artikli = m_ArtikliRepository.VratiSveArtikle();
            //foreach (var artikal in artikli)
            //{
            //    artikal.Dobavljac = null;
            //    var naziv = artikal.Naziv.Replace('а', 'a');
            //    naziv = naziv.Replace('А', 'A');
            //    artikal.Naziv = naziv;
            //    m_ArtikliRepository.Sacuvaj(artikal);
            //}
            m_Index.IndeksirajArtikle(artikli);
            OnAlertChanged(new AlertEventArgs("Indeksiranje završeno"));
        }

        private void PretraziPoDeluNaziva()
        {
            m_Artikli = new BindingList<SlicnostArtikla>();
            var deoNaziva = textEdit1.Text;
            var artikli = m_Index.PretraziArtikle(deoNaziva, checkEdit2.Checked);
            if (checkEdit1.Checked)
            {
                artikli = artikli.Where(x => x.IdSinonima == null).ToList();
            }
            m_Artikli = new BindingList<SlicnostArtikla>(artikli.Select(x => new SlicnostArtikla(x)).ToList());
            slicnostArtiklaBindingSource.DataSource = m_Artikli;
            UcitajSlike();
        }

        private void UcitajSlike()
        {
            lock (m_Slike)
            {
                m_Slike.Clear();
                var worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    foreach (var artikal in m_Artikli)
                    {
                        if (artikal.Slika != null)
                        {
                            var slika = m_StorageService.VratiSlikuArtikla(artikal.Slika);
                            SlicnostArtikla artikal1 = artikal;
                            gridControl1.Invoke(new Action(() => artikal1.SlikaRaw = slika));
                            if (!m_Slike.ContainsKey(artikal.Id))
                            {
                                m_Slike.Add(artikal.Id, slika);
                            }
                        }
                    }
                };
                worker.RunWorkerAsync();
            }
        }
    }
}
