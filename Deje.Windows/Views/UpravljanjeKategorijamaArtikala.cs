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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Deje.Windows.Views
{
    public partial class UpravljanjeKategorijamaArtikala : ViewBase
    {
        private readonly KategorijeArtikalaRepository m_KategorijeArtikalaRepository = new KategorijeArtikalaRepository();

        private readonly ArtikliRepository m_ArtikliRepository = new ArtikliRepository();

        private readonly IPictureStorageService m_Storage = new PictureStorageService();

        private BindingList<Artikal> m_Artikli1 = new BindingList<Artikal>();

        private BindingList<Artikal> m_Artikli2 = new BindingList<Artikal>();

        private List<int> m_SelektovaniArtikli1 = new List<int>();

        private List<int> m_SelektovaniArtikli2 = new List<int>();

        public UpravljanjeKategorijamaArtikala()
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
            m_SelektovaniArtikli1.Clear();
            m_SelektovaniArtikli2.Clear();
            var kategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve().OrderBy(x => x.Naziv);
            kategorijaArtiklaBindingSource.DataSource = kategorijeArtikala;
        }

        private void UcitajArtikle1() 
        {
            if (lookUpEdit1.EditValue != null) 
            {
                var id = (int)lookUpEdit1.EditValue;
                var artikli = UcitajArtikle(id);
                m_Artikli1 = new BindingList<Artikal>(artikli);
                artikalBindingSource.DataSource = m_Artikli1;
            }
        }

        private void UcitajArtikle2() 
        {
            if (lookUpEdit2.EditValue != null) 
            {
                var id = (int)lookUpEdit2.EditValue;
                var artikli = UcitajArtikle(id);
                m_Artikli2 = new BindingList<Artikal>(artikli);
                artikalBindingSource1.DataSource = m_Artikli2;
            }
        }

        private IList<Core.Model.Artikal> UcitajArtikle(int id) 
        {
            var artikli = m_ArtikliRepository.VratiArtikleKategorije(id);
            foreach(var artikal in artikli) 
            {
                if (artikal.Slika != null)
                {
                    artikal.SlikaRaw = m_Storage.VratiSlikuArtikla(artikal.Slika);
                }
            }
            return artikli;
        }

        private void RegisterEvents()
        {
            lookUpEdit1.EditValueChanged += (s, e) => UcitajArtikle1();
            lookUpEdit2.EditValueChanged += (s, e) => UcitajArtikle2();
            gridView1.CustomUnboundColumnData += (s, e) =>
            {
                if (e.Column.FieldName == "selektovan")
                {
                    if (e.IsGetData)
                    {
                        var artikal = m_Artikli1[e.ListSourceRowIndex];
                        e.Value = m_SelektovaniArtikli1.Any(x => x == artikal.Id);
                    }
                    if (e.IsSetData)
                    {
                        var artikal = m_Artikli1[e.ListSourceRowIndex];
                        var value = Convert.ToBoolean(e.Value);
                        if (value)
                        {
                            m_SelektovaniArtikli1.Add(artikal.Id);
                        }
                        else
                        {
                            m_SelektovaniArtikli1.Remove(artikal.Id);
                        }
                    }
                }
            };
            gridView2.CustomUnboundColumnData += (s, e) =>
            {
                if (e.Column.FieldName == "selektovan")
                {
                    if (e.IsGetData)
                    {
                        var artikal = m_Artikli2[e.ListSourceRowIndex];
                        e.Value = m_SelektovaniArtikli2.Any(x => x == artikal.Id);
                    }
                    if (e.IsSetData)
                    {
                        var artikal = m_Artikli2[e.ListSourceRowIndex];
                        var value = Convert.ToBoolean(e.Value);
                        if (value)
                        {
                            m_SelektovaniArtikli2.Add(artikal.Id);
                        }
                        else
                        {
                            m_SelektovaniArtikli2.Remove(artikal.Id);
                        }
                    }
                }
            };
            btnToRight.Click += (s, e) =>
            {
                if (lookUpEdit2.EditValue != null)
                {
                    foreach (var id in m_SelektovaniArtikli1.ToArray())
                    {
                        var artikal = m_Artikli1.Single(x => x.Id == id);
                        artikal.IdKategorijeArtikla = (int)lookUpEdit2.EditValue;
                        m_ArtikliRepository.Sacuvaj(artikal);
                        m_Artikli2.Add(artikal);
                        m_Artikli1.Remove(artikal);
                        m_SelektovaniArtikli1.Remove(artikal.Id);
                    }
                }
            };
            btnToLeft.Click += (s, e) =>
            {
                if (lookUpEdit1.EditValue != null)
                {
                    foreach (var id in m_SelektovaniArtikli2.ToArray())
                    {
                        var artikal = m_Artikli2.Single(x => x.Id == id);
                        artikal.IdKategorijeArtikla = (int)lookUpEdit1.EditValue;
                        m_ArtikliRepository.Sacuvaj(artikal);
                        m_Artikli1.Add(artikal);
                        m_Artikli2.Remove(artikal);
                        m_SelektovaniArtikli2.Remove(artikal.Id);
                    }
                }
            };
            btnToRightAll.Click += (s, e) =>
            {
                if (lookUpEdit2.EditValue != null)
                {
                    foreach (var artikal in m_Artikli1.ToArray())
                    {
                        artikal.IdKategorijeArtikla = (int)lookUpEdit2.EditValue;
                        m_ArtikliRepository.Sacuvaj(artikal);
                        m_Artikli2.Add(artikal);
                        m_Artikli1.Remove(artikal);
                        m_SelektovaniArtikli1.Remove(artikal.Id);
                    }
                }
            };
            btnToLeftAll.Click += (s, e) =>
            {
                if (lookUpEdit1.EditValue != null)
                {
                    foreach (var artikal in m_Artikli2.ToArray())
                    {
                        artikal.IdKategorijeArtikla = (int)lookUpEdit1.EditValue;
                        m_ArtikliRepository.Sacuvaj(artikal);
                        m_Artikli1.Add(artikal);
                        m_Artikli2.Remove(artikal);
                        m_SelektovaniArtikli2.Remove(artikal.Id);
                    }
                }
            };
            btnObrisi1.Click += (s, e) =>
            { 
                if (lookUpEdit1.EditValue != null) 
                {
                    m_KategorijeArtikalaRepository.Obrisi((int)lookUpEdit1.EditValue);
                }
                OnRefresh();
            };
            btnObrisi2.Click += (s, e) =>
            {
                if (lookUpEdit2.EditValue != null)
                {
                    m_KategorijeArtikalaRepository.Obrisi((int)lookUpEdit2.EditValue);
                }
                OnRefresh();
            };
        }
    }
}
