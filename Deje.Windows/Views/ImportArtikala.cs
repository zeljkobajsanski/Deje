using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Deje.Windows.Model;
using Deje.Windows.Services.ImportArtikala;
using Shell;

namespace Deje.Windows.Views
{
    public partial class ImportArtikala : ViewBase
    {
        private BindingList<Artikal> m_Artikli = new BindingList<Artikal>();

        public ImportArtikala()
        {
            InitializeComponent();
            btnOpen.Click += (s, e) =>
            {
                if (!String.IsNullOrEmpty(textEdit1.Text))
                {
                    Uvezi();
                }
            };
        }

        private void Uvezi()
        {
            var url = textEdit1.Text;
            IImportArtiakalaService importService = null;

            if (url.StartsWith("http://www.donesi.com"))
            {
                importService = new DonesiComImportService();
            }

            if (importService == null) throw new Exception("Importer za adresu: " + url + " nije definisan");
            var artikli = importService.Importuj(url);
            m_Artikli = new BindingList<Artikal>(artikli);
            artikalBindingSource.DataSource = m_Artikli;
        }

        public override void OnSave()
        {
            if (DialogResult.OK == saveFileDialog1.ShowDialog(this))
            {
                using (var s = saveFileDialog1.OpenFile())
                {
                    var ser = new XmlSerializer(typeof (Artikal[]));
                    ser.Serialize(s, m_Artikli.ToArray());
                }
            }
        }
    }
}
