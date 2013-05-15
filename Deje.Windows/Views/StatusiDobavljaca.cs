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
    public partial class StatusiDobavljaca : ViewBase
    {
        private readonly StatusiRepository m_StatusiRepository = new StatusiRepository();

        private BindingList<StatusDobavljaca> m_Statusi = new BindingList<StatusDobavljaca>(); 

        public StatusiDobavljaca()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            OnRefresh();
        }

        public override void OnRefresh()
        {
            m_Statusi = new BindingList<StatusDobavljaca>(m_StatusiRepository.VratiStatuseDobavljaca().ToList());
            statusDobavljacaBindingSource.DataSource = m_Statusi;
        }

        public override void OnSave()
        {
            foreach (var statusDobavljaca in m_Statusi)
            {
                m_StatusiRepository.Save(statusDobavljaca);
            }
            OnRefresh();
        }

        public override void OnNew()
        {
            var novi = m_Statusi.AddNew();
            statusDobavljacaBindingSource.Position = statusDobavljacaBindingSource.IndexOf(novi);
        }

        //public override void OnDelete()
        //{
        //    var status = gridView1.GetFocusedRow() as StatusDobavljaca;
        //    if (status != null)
        //    {
                
        //    }
        //}
    }
}
