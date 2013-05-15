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
using System.Xml.Serialization;
using Deje.AzureBlobStorage;
using Deje.Core.Model;
using Deje.Core.Services;
using Deje.Repository.EF.Repository;
using Shell;
using Shell.Events;
using Artikal = Deje.Windows.Model.Artikal;

namespace Deje.Windows.Views
{
    public partial class ArtikliDobavljaca : ViewBase
    {
        private readonly ArtikliRepository m_ArtikliRepository = new ArtikliRepository();

        private readonly DobavljaciRepository m_DobavljaciRepository = new DobavljaciRepository();

        private readonly KategorijeArtikalaRepository m_KategorijeArtikalaRepository = new KategorijeArtikalaRepository();

        private BindingList<Core.Model.Artikal> m_Artikli = new BindingList<Core.Model.Artikal>(); 

        private readonly IPictureStorageService m_PictureStorageService = new PictureStorageService();

        private Dictionary<int, byte[]> m_Slike = new Dictionary<int, byte[]>();

        public ArtikliDobavljaca()
        {
            InitializeComponent();
            simpleButton1.Click += (s, e) => Uvezi();
            lookUpEdit1.EditValueChanged += (s, e) => UcitajArtikle();
        }

        private void UcitajArtikle()
        {
            if (IzabraniDobavljac != null)
            {
                m_Artikli = new BindingList<Core.Model.Artikal>(m_ArtikliRepository.VratiArtikleDobaljvaca(IzabraniDobavljac.Id)
                    .OrderBy(x => x.Id).ToList());
                foreach (var artikal in m_Artikli)
                {
                    if (artikal.Slika != null)
                    {
                        artikal.SlikaRaw = m_PictureStorageService.VratiSlikuArtikla(artikal.Slika);
                        if (!m_Slike.ContainsKey(artikal.Id))
                        {
                            m_Slike.Add(artikal.Id, artikal.SlikaRaw);
                        }
                    }
                }
                artikalBindingSource.DataSource = m_Artikli;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            OnRefresh();
        }

        public override void OnRefresh()
        {
            var izabraniDobavljac = IzabraniDobavljac;
            var dobavljaci = m_DobavljaciRepository.VratiSve().OrderBy(x => x.Naziv);
            dobavljacBindingSource.DataSource = dobavljaci;
            lookUpEdit1.EditValue = izabraniDobavljac != null ? dobavljaci.Single(x => x.Id == izabraniDobavljac.Id) : null;
            kategorijaArtiklaBindingSource.DataSource = m_KategorijeArtikalaRepository.VratiSve().OrderBy(x => x.Naziv);
            m_Slike.Clear();
        }

        public override void OnNew()
        {
            if (IzabraniDobavljac == null)
            {
                OnAlertChanged(new AlertEventArgs("Izaberite prvo dobavljača"));
                return;
            }
            var artikal = new Core.Model.Artikal() { IdDobavljaca = IzabraniDobavljac.Id, Aktivan = true };
            artikalBindingSource.DataSource = artikal;
        }

        public override void OnSave()
        {
            var artikal = artikalBindingSource.Current as Core.Model.Artikal;
            if (artikal != null)
            {
                if (artikal.SlikaRaw != null && artikal.SlikaRaw.Length != 0)
                {
                    if (m_Slike.ContainsKey(artikal.Id))
                    {
                        if (!artikal.SlikaRaw.SequenceEqual(m_Slike[artikal.Id]))
                        {
                            var putanja = m_PictureStorageService.SacuvajSlikuArtikla(new MemoryStream(artikal.SlikaRaw), "image/jpeg", ".jpg");
                            artikal.Slika = putanja;
                        }
                    }
                    if (!m_Slike.ContainsKey(artikal.Id))
                    {
                        var putanja = m_PictureStorageService.SacuvajSlikuArtikla(new MemoryStream(artikal.SlikaRaw), "image/jpeg", ".jpg");
                        artikal.Slika = putanja;
                    }
                }
                m_ArtikliRepository.Sacuvaj(artikal);
            }
        }

        private void Uvezi()
        {
            if (IzabraniDobavljac == null)
            {
                OnAlertChanged(new AlertEventArgs("Izaberite prvo dobavljača"));
                return;
            }
            if (DialogResult.OK == openFileDialog1.ShowDialog(this))
            {
                using (var s = openFileDialog1.OpenFile())
                {
                    var ser = new XmlSerializer(typeof (Artikal[]));
                    var artikli = (Artikal[])ser.Deserialize(s);
                    foreach (var artikal in artikli)
                    {
                        var idKategorije = m_KategorijeArtikalaRepository.Sacuvaj(artikal.Kategorija);
                        var a = new Core.Model.Artikal
                        {
                            IdDobavljaca = IzabraniDobavljac.Id,
                            Naziv = artikal.Naziv,
                            Opis = artikal.Opis,
                            Cena = artikal.Cena,
                            IdKategorijeArtikla = idKategorije,
                            Aktivan = true
                        };
                        m_ArtikliRepository.Sacuvaj(a);
                    }
                    OnAlertChanged(new AlertEventArgs("Artikli su uspešno uvezeni"));
                    OnRefresh();
                }
            }
        }

        private Dobavljac IzabraniDobavljac
        {
            get
            {
                if (lookUpEdit1.EditValue != null && lookUpEdit1.EditValue != DBNull.Value)
                {
                   return (Dobavljac)lookUpEdit1.EditValue;
                }
                return null;
            }
        }
    }
}
