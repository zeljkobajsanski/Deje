using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Deje.AzureBlobStorage;
using Deje.Core.Model;
using Deje.Core.Services;
using Deje.Repository.EF.Repository;
using Shell;
using Deje.Windows.Utils;

namespace Deje.Windows.Views
{
    public partial class Dobavljaci : ViewBase
    {
        private readonly DobavljaciRepository m_DobavljaciRepository = new DobavljaciRepository();

        private readonly DelatnostiRepository m_DelatnostiRepository = new DelatnostiRepository();

        private readonly VrsteDobavljacaRepository m_VrsteDobavljacaRepository = new VrsteDobavljacaRepository();

        private readonly IPictureStorageService m_PictureStorageService = new PictureStorageService();

        private readonly Dictionary<int, byte[]> m_Slike = new Dictionary<int, byte[]>(); 

        private readonly StatusiRepository m_StatusiRepository = new StatusiRepository();

        public Dobavljaci()
        {
            InitializeComponent();
            lookUpEdit1.EditValueChanged += (s, e) => UcitajDobavljaca();
            lookUpEdit2.EditValueChanged += (s, e) => UcitajVrsteDobavljaca();
            webBrowser1.Url = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Mapa.html");
            textEdit3.Validated += (s, e) => PretraziAdresu();
            textEdit4.Validated += (s, e) => PretraziAdresu();
            simpleButton1.Click += (s, e) => KonvertujOpis();
        }

        

        private void UcitajVrsteDobavljaca()
        {
            var idDelatnosti = lookUpEdit2.EditValue;
            if (idDelatnosti != null && idDelatnosti != DBNull.Value)
            {
                vrstaDobavljacaBindingSource.DataSource = m_VrsteDobavljacaRepository.VratiZaDelatnost((int)idDelatnosti).OrderBy(x => x.Naziv);
            }
        }

        private void UcitajDobavljaca()
        {
            try
            {
                textEdit3.Properties.LockEvents();
                textEdit4.Properties.LockEvents();
                if (lookUpEdit1.EditValue == null || lookUpEdit1.EditValue == DBNull.Value) return;
                var dobavljac = (Dobavljac)lookUpEdit1.EditValue;
                if (dobavljac.SlikaRaw == null && dobavljac.Slika != null)
                {
                    dobavljac.SlikaRaw = m_PictureStorageService.VratiSlikuDobavljaca(dobavljac.Slika);
                    m_Slike.Add(dobavljac.Id, dobavljac.SlikaRaw);
                }
                dobavljac.Status = null;
                dobavljacBindingSource.DataSource = dobavljac;
                kontaktBindingSource.DataSource = dobavljac.Kontakt;
                PrikaziNaMapi(dobavljac);
            }
            finally
            {
                textEdit3.Properties.UnLockEvents();
                textEdit4.Properties.UnLockEvents();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            OnRefresh();
        }

        public override void OnRefresh()
        {
            m_Slike.Clear();
            var dobavljaci = m_DobavljaciRepository.VratiSve().OrderBy(x => x.Id).ToList();
            dobavljaciBindingSource.DataSource = dobavljaci;
            delatnostBindingSource.DataSource = m_DelatnostiRepository.VratiSve().OrderBy(x => x.Naziv);
            statusDobavljacaBindingSource.DataSource = m_StatusiRepository.VratiStatuseDobavljaca().OrderBy(x => x.Naziv);
        }

        public override void OnSave()
        {
            var dobavljac = dobavljacBindingSource.DataSource as Dobavljac;
            if (dobavljac != null)
            {
                var strLatituda = webBrowser1.Document.GetElementById("latituda").GetAttribute("value");
                var strLongituda = webBrowser1.Document.GetElementById("longituda").GetAttribute("value");
                var strZoom = webBrowser1.Document.GetElementById("zoom").GetAttribute("value");
                var latituda = 0.00;
                var longituda = 0.00;
                var zoom = 0;

                double.TryParse(strLatituda, out latituda);
                double.TryParse(strLongituda, out longituda);
                int.TryParse(strZoom, out zoom);

                dobavljac.GpsLatitude = latituda;
                dobavljac.GpsLongitude = longituda;
                dobavljac.Zoom = zoom;

                if (dobavljac.Id == 0)
                {
                    if (dobavljac.SlikaRaw != null && dobavljac.SlikaRaw.Length != 0)
                    {
                        SacuvajSlikuDobavljaca(dobavljac);
                    }
                } else
                {
                    if (m_Slike.ContainsKey(dobavljac.Id))
                    {
                        if (!m_Slike[dobavljac.Id].SequenceEqual(dobavljac.SlikaRaw))
                        {
                            SacuvajSlikuDobavljaca(dobavljac);    
                        }
                    } else
                    {
                        if (dobavljac.SlikaRaw != null && dobavljac.SlikaRaw.Length != 0)
                        {
                            SacuvajSlikuDobavljaca(dobavljac);
                        }
                    }
                }
                m_DobavljaciRepository.Save(dobavljac);
            }
            OnRefresh();
        }

        public override void OnNew()
        {
            var novi = (Dobavljac)dobavljaciBindingSource.AddNew();
            //novi.GpsLatitude = 45.15;
            //novi.GpsLongitude = 19.85;
            novi.Kontakt = new Kontakt();
            dobavljacBindingSource.DataSource = novi;
            kontaktBindingSource.DataSource = novi.Kontakt;

            PrikaziNaMapi(novi);
        }

        private void PrikaziNaMapi(Dobavljac dobavljac)
        {
            webBrowser1.Document.GetElementById("latituda").SetAttribute("value", dobavljac.GpsLatitude.ToString());
            webBrowser1.Document.GetElementById("longituda").SetAttribute("value", dobavljac.GpsLongitude.ToString());
            webBrowser1.Document.GetElementById("zoom").SetAttribute("value", dobavljac.Zoom.ToString());
            webBrowser1.Document.GetElementById("refresh").InvokeMember("click");
        }

        private void SacuvajSlikuDobavljaca(Dobavljac dobavljac)
        {
            var putanja = m_PictureStorageService.SacuvajSlikuDobavljaca(new MemoryStream(dobavljac.SlikaRaw),
                                                                         "image/jpeg", ".jpg");
            dobavljac.Slika = putanja;
        }

        private Dobavljac Dobavljac
        {
            get
            {
                var dobavljac = dobavljacBindingSource.DataSource as Dobavljac;
                return dobavljac;
            }
        }

        private void PretraziAdresu()
        {
            var web = new WebClient();
            var uri = "http://dev.virtualearth.net/REST/v1/Locations?query=" + textEdit3.Text + " " + textEdit4.Text +
                      "&output=xml&key=AoAspjo-51Af3fRuiPmgnwZ6969gydoWrZ26rmvfE9tk10e7Q5JYNvs4rXLQ8VKn";
            web.DownloadStringCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    var result = e.Result;
                    XDocument doc = XDocument.Parse(result);
                    var points = doc.Descendants().Where(x => x.Name.LocalName == "Point");
                    if (points.Count() == 1)
                    {
                        var latitude = points.Single().Descendants().Single(x => x.Name.LocalName == "Latitude").Value;
                        var longitude = points.Single().Descendants().Single(x => x.Name.LocalName == "Longitude").Value;
                        if (Dobavljac != null)
                        {
                            Dobavljac.GpsLatitude = Double.Parse(latitude);
                            Dobavljac.GpsLongitude = Double.Parse(longitude);
                            PrikaziNaMapi(Dobavljac);
                        }
                    }
                }
            };
            web.DownloadStringAsync(new Uri(uri));
        }

        private void KonvertujOpis()
        {
            if (Dobavljac != null && Dobavljac.Opis != null)
            {
                Dobavljac.Opis = Dobavljac.Opis.ConvertFromCyrilic();
            }
        }
    }


}
