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
    public partial class Delatnosti : ViewBase
    {
        private readonly DelatnostiRepository m_DelatnostiRepository = new DelatnostiRepository();

        private BindingList<Delatnost> m_Delatnosti = new BindingList<Delatnost>();

        public Delatnosti()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            OnRefresh();
        }

        public override void OnSave()
        {
            foreach (var delatnost in m_Delatnosti)
            {
                m_DelatnostiRepository.Save(delatnost);
            }
            OnRefresh();
        }

        public override void OnNew()
        {
            m_Delatnosti.AddNew();
        }

        public override void OnRefresh()
        {
            m_Delatnosti = new BindingList<Delatnost>(m_DelatnostiRepository.VratiSve().OrderBy(x => x.Id).ToList());
            delatnostBindingSource.DataSource = m_Delatnosti;
        }

        public override void OnDelete()
        {
            var delatnost = gridView1.GetFocusedRow() as Delatnost;
            if (delatnost != null)
            {
                m_DelatnostiRepository.Delete(delatnost.Id);
            }
            OnRefresh();
        }
    }
}
