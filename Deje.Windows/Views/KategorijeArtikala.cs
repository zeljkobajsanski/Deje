using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Deje.Core.Model;
using Deje.Repository.EF.Repository;
using Shell;

namespace Deje.Windows.Views
{
    public partial class KategorijeArtikala : ViewBase
    {
        private readonly KategorijeArtikalaRepository m_KategorijeArtikalaRepository = new KategorijeArtikalaRepository();

        private readonly BindingList<KategorijaArtikla> m_KategorijeArtikala = new BindingList<KategorijaArtikla>();

        public KategorijeArtikala()
        {
            InitializeComponent();
            kategorijaArtiklaBindingSource.DataSource = m_KategorijeArtikala;
        }

        protected override void OnLoad(EventArgs e)
        {
            OnRefresh();
        }

        public override void OnRefresh()
        {
            m_KategorijeArtikala.Clear();
            var kategorije = m_KategorijeArtikalaRepository.VratiSve().OrderBy(x => x.Id);
            foreach (var kategorijaArtikla in kategorije)
            {
                m_KategorijeArtikala.Add(kategorijaArtikla);
            }
        }

        public override void OnSave()
        {
            gridView1.CloseEditor();
            foreach (var kategorijaArtikla in m_KategorijeArtikala)
            {
                m_KategorijeArtikalaRepository.Sacuvaj(kategorijaArtikla);
            }
            OnRefresh();
        }

        public override void OnNew()
        {
            var nova = m_KategorijeArtikala.AddNew();
            kategorijaArtiklaBindingSource.Position = kategorijaArtiklaBindingSource.IndexOf(nova);
        }

        public override void OnDelete()
        {
            var k = gridView1.GetFocusedRow() as KategorijaArtikla;
            if (k != null)
            {
                m_KategorijeArtikalaRepository.Obrisi(k.Id);
            }
            OnRefresh();
        }
    }
}
