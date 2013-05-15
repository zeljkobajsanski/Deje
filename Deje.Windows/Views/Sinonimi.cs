using Deje.AzureBlobStorage;
using Deje.Core.Model;
using Deje.Core.Services;
using Deje.Repository.EF.Repository;
using Shell;
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
using Deje.Windows.Utils;

namespace Deje.Windows.Views
{
    public partial class Sinonimi : ViewBase
    {
        private readonly ArtikliRepository m_ArtikliRepository = new ArtikliRepository();

        private readonly KategorijeArtikalaRepository m_KategorijeArtikalaRepository = new KategorijeArtikalaRepository();

        private BindingList<Sinonim> m_Sinonimi = new BindingList<Sinonim>();

        private BindingList<Artikal> m_ArtikliSinonima = new BindingList<Artikal>();

        private readonly IPictureStorageService m_Storage = new PictureStorageService();

        private readonly Dictionary<int, byte[]> m_Slike = new Dictionary<int, byte[]>();
        
        private readonly Dictionary<int, byte[]> m_SlikeArtikala = new Dictionary<int, byte[]>();

        public Sinonimi()
        {
            InitializeComponent();
            RegisterEvents();
        }

        protected override void OnLoad(EventArgs e)
        {
            OnRefresh();
        }

        public override void OnRefresh()
        {
            UcitajSinonime();
            UcitajArtikle();
            kategorijaArtiklaBindingSource.DataSource = m_KategorijeArtikalaRepository.VratiSve().OrderBy(x => x.Naziv);
        }

        public override void OnNew()
        {
            var sinonim = m_Sinonimi.AddNew();
            sinonimBindingSource.MoveTo(sinonim);
        }

        public override void OnSave()
        {
            gridView1.CloseEditor();
            gridView2.CloseEditor();
            var sinonim = Sinonim;
            if (sinonim != null)
            {
                if (sinonim.SlikaRaw != null)
                {
                    if (sinonim.Id == 0)
                    {
                        sinonim.Slika = m_Storage.SacuvajSlikuArtikla(new MemoryStream(sinonim.SlikaRaw), "image/jpeg", ".jpg");
                    }
                    else
                    {
                        if (!m_Slike.ContainsKey(sinonim.Id))
                        {
                            sinonim.Slika = m_Storage.SacuvajSlikuArtikla(new MemoryStream(sinonim.SlikaRaw), "image/jpeg", ".jpg");
                        }
                        else
                        {
                            if (!m_Slike[sinonim.Id].SequenceEqual(sinonim.SlikaRaw)) {
                                sinonim.Slika = m_Storage.SacuvajSlikuArtikla(new MemoryStream(sinonim.SlikaRaw), "image/jpeg", ".jpg");
                            }
                        }
                    }
                }
                m_ArtikliRepository.SacuvajSinonim(sinonim);
            }
            foreach (var artikal in m_ArtikliSinonima)
            {
                if (artikal.SlikaRaw != null)
                {
                    if (m_SlikeArtikala.ContainsKey(artikal.Id))
                    {
                        if (!m_SlikeArtikala[artikal.Id].SequenceEqual(artikal.SlikaRaw))
                        {
                            artikal.Slika = m_Storage.SacuvajSlikuArtikla(new MemoryStream(artikal.SlikaRaw), "image/jpeg", ".jpg");
                        }
                    } else
                    {
                        artikal.Slika = m_Storage.SacuvajSlikuArtikla(new MemoryStream(artikal.SlikaRaw), "image/jpeg", ".jpg");
                        m_SlikeArtikala.Add(artikal.Id, artikal.SlikaRaw);
                    }
                }
                m_ArtikliRepository.Sacuvaj(artikal);
            }
            UcitajSinonime();
            UcitajArtikle();
        }

        public override void OnDelete()
        {
            if (gridView1.IsFocusedView && Sinonim != null)
            {
                m_ArtikliRepository.ObrisiSinonim(Sinonim.Id);
                UcitajSinonime();
                UcitajArtikle();
            }
            if (gridView2.IsFocusedView)
            {
                var artikal = gridView2.GetFocusedRow() as Artikal;
                artikal.IdSinonima = null;
                m_ArtikliRepository.Sacuvaj(artikal);
                m_ArtikliSinonima.Remove(artikal);
                UcitajArtikle();
            }
        }

        private void RegisterEvents()
        {
            simpleButton1.Click += (s, e) =>
                {
                    if (lookUpEdit1.EditValue != null && Sinonim != null)
                    {
                        var artikal = lookUpEdit1.EditValue as Artikal;
                        artikal.KategorijaArtikla = null;
                        artikal.IdSinonima = Sinonim.Id;
                        m_ArtikliSinonima.Add(artikal);
                    }
                };
            gridView1.FocusedRowChanged += (s, e) =>
                {
                    if (Sinonim != null)
                    {
                        UcitajArtikleSinonima();
                    }
                };
        }

        private void UcitajSinonime()
        {
            m_Slike.Clear();
            var sinonimi = m_ArtikliRepository.VratiSinonime().OrderBy(x => x.Id).ToList();
            
            m_Sinonimi = new BindingList<Sinonim>(sinonimi);
            sinonimBindingSource.DataSource = m_Sinonimi;
            UcitajSlikeSinonima();
        }

        

        private Sinonim Sinonim
        {
            get { return gridView1.GetFocusedRow() as Sinonim; }
        }

        private void UcitajArtikle()
        {
            artikalBindingSource1.DataSource = m_ArtikliRepository.VratiArtikleBezSinonima().OrderBy(x => x.Naziv);
        }

        private void UcitajArtikleSinonima()
        {
            if (Sinonim != null)
            {
                m_SlikeArtikala.Clear();
                var artikli = m_ArtikliRepository.VratiArtikleSinonima(Sinonim.Id).OrderBy(x => x.Naziv).ToList();
                
                m_ArtikliSinonima = new BindingList<Artikal>(artikli);
                artikalBindingSource.DataSource = m_ArtikliSinonima;
                UcitajSlikeArtikala();
            }
        }

        private void UcitajSlikeArtikala()
        {
            lock (m_SlikeArtikala)
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    foreach (var artikal in m_ArtikliSinonima)
                    {
                        if (artikal.Slika != null)
                        {
                            m_SlikeArtikala.Add(artikal.Id, artikal.SlikaRaw);
                            Artikal artikal1 = artikal;
                            gridControl2.Invoke(new Action(() => { artikal1.SlikaRaw = m_Storage.VratiSlikuArtikla(artikal1.Slika); }));

                        }
                    }
                };
                worker.RunWorkerAsync();
            }
        }

        private void UcitajSlikeSinonima()
        {
            lock (m_Slike)
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    foreach (var sinonim in m_Sinonimi)
                    {
                        if (sinonim.Slika != null)
                        {
                            var slika = m_Storage.VratiSlikuArtikla(sinonim.Slika);
                            Sinonim sinonim1 = sinonim;
                            gridControl1.Invoke(new Action(() => { sinonim1.SlikaRaw = slika; }));
                            m_Slike.Add(sinonim.Id, slika);
                        }
                    }
                };
                worker.RunWorkerAsync();
            }
        }
    }
}
