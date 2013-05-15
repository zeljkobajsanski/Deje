using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Deje.AzureBlobStorage;
using Deje.Repository.EF.Repository;
using Deje.Windows.Model;
using DevExpress.XtraEditors.Controls;
using Shell;
using SimMetricsMetricUtilities;
using Artikal = Deje.Core.Model.Artikal;

namespace Deje.Windows.Views
{
    public partial class Artikli : ViewBase
    {
        private readonly KategorijeArtikalaRepository m_KategorijeArtikalaRepository = new KategorijeArtikalaRepository();

        private readonly ArtikliRepository m_ArtikliRepository = new ArtikliRepository();

        private readonly PictureStorageService m_StorageService = new PictureStorageService();

        private readonly Dictionary<int, byte[]> m_Slike = new Dictionary<int, byte[]>(); 

        private BindingList<Artikal> m_Artikli = new BindingList<Artikal>(); 

        public Artikli()
        {
            InitializeComponent();
            lookUpEdit1.EditValueChanged += (s, e) => Prikazi();
            lookUpEdit1.ButtonClick += (s, e) =>
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    lookUpEdit1.EditValue = null;
                }
            };
            simpleButton1.Click += (s, e) => Prikazi();
        }

        private void Prikazi()
        {
            if (lookUpEdit1.EditValue != null)
            {
                var id = (int) lookUpEdit1.EditValue;
                UcitajArtikle(id);
            }
            else
            {
                UcitajArtikle(null);
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            OnRefresh();
        }

        public override void OnRefresh()
        {
            
            kategorijaArtiklaBindingSource.DataSource = m_KategorijeArtikalaRepository.VratiSve();
            sinonimBindingSource.DataSource = m_ArtikliRepository.VratiSinonime().OrderBy(x => x.Naziv);
        }

        public override void OnSave()
        {
            gridView1.CloseEditor();
            var artikal = gridView1.GetFocusedRow() as Artikal;
            if (artikal == null) return;
            if (artikal.SlikaRaw != null)
            {
                if (m_Slike.ContainsKey(artikal.Id))
                {
                    if (!m_Slike[artikal.Id].SequenceEqual(artikal.SlikaRaw))
                    {
                        artikal.Slika = m_StorageService.SacuvajSlikuArtikla(new MemoryStream(artikal.SlikaRaw), "image/jpeg", ".jpg");
                        m_Slike[artikal.Id] = artikal.SlikaRaw;
                    }
                } else
                {
                    artikal.Slika = m_StorageService.SacuvajSlikuArtikla(new MemoryStream(artikal.SlikaRaw), "image/jpeg", ".jpg");
                    m_Slike.Add(artikal.Id, artikal.SlikaRaw);
                }
            }
            m_ArtikliRepository.Sacuvaj(artikal);
        }

        private void UcitajArtikle(int? idKategorije)
        {
            IEnumerable<Artikal> artikli = !idKategorije.HasValue
                                               ? m_ArtikliRepository.VratiSveArtikle()
                                               : m_ArtikliRepository.VratiArtikleKategorije(idKategorije.Value);
            if (checkEdit1.Checked)
            {
                artikli = artikli.Where(x => !x.IdSinonima.HasValue);
            } else
            {
                artikli = artikli.Where(x => x.IdSinonima.HasValue);
            }
            m_Artikli = new BindingList<Artikal>(artikli.ToList());
            
            
            
            artikalBindingSource.DataSource = m_Artikli;
            UcitajSlikeArtikala();
        }

        private void UcitajSlikeArtikala()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                lock (m_Slike)
                {
                    foreach (var artikal in m_Artikli)
                    {
                        Artikal artikal1 = artikal;
                        artikal1.Dobavljac = null;
                        artikal1.KategorijaArtikla = null;
                        if (artikal1.Slika != null)
                        {
                            var slika = m_StorageService.VratiSlikuArtikla(artikal1.Slika);
                            if (!m_Slike.ContainsKey(artikal1.Id))
                            {
                                m_Slike.Add(artikal1.Id, slika);
                            }

                            gridControl1.Invoke(new Action(() => { artikal1.SlikaRaw = slika; }));
                        }
                    }    
                }
            };
            worker.RunWorkerAsync();
        }
    }
}
