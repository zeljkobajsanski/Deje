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
using Deje.Lucene;
using Deje.Repository.EF.Repository;
using Shell;
using Shell.Events;

namespace Deje.Windows.Views
{
    public partial class Sinonimi2 : ViewBase
    {
        private string m_File;

        public Sinonimi2()
        {
            InitializeComponent();
            simpleButton1.Click += (s, e) => Otvori();
            simpleButton2.Click += (s, e) => Indeksiraj();
        }

        public override void OnSave()
        {
            if (m_File != null)
            {
                using (var sw = new StreamWriter(m_File))
                {
                    var chars = memoEdit1.Text.ToCharArray();
                    sw.Write(chars);
                }
            }
        }

        private void Otvori()
        {
            var res = openFileDialog1.ShowDialog(this);
            if (DialogResult.OK == res)
            {
                m_File = openFileDialog1.FileName;
                using (var s = openFileDialog1.OpenFile())
                {
                    var strS = new StreamReader(s);
                    memoEdit1.Text = strS.ReadToEnd();
                }
            }
        }

        private void Indeksiraj()
        {
            var repository = new ArtikliRepository();
            var artikli = repository.VratiSveArtikle();
            var index = new Index();
            index.ObrisiIndekse();
            index.IndeksirajArtikle(artikli);
            var sinonimi = repository.VratiSinonime();
            index.IndeksirajSinonime(sinonimi);
            OnAlertChanged(new AlertEventArgs("Indeksiranje završeno"));
        }

        
    }
}
