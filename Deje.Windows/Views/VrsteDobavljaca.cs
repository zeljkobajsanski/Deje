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
    public partial class VrsteDobavljaca : ViewBase
    {
        private readonly DelatnostiRepository m_DelatnostiRepository = new DelatnostiRepository();

        private readonly VrsteDobavljacaRepository m_VrsteDobavljacaRepository = new VrsteDobavljacaRepository();

        private readonly BindingList<VrstaDobavljaca> m_VrsteDobavljaca = new BindingList<VrstaDobavljaca>();

        public VrsteDobavljaca()
        {
            InitializeComponent();
            lookUpEdit1.EditValueChanged += (s, e) => UcitajVrsteDobavljaca();
        }

        protected override void OnLoad(EventArgs e)
        {
            OnRefresh();
            vrstaDobavljacaBindingSource.DataSource = m_VrsteDobavljaca;
        }

        public override void OnRefresh()
        {
            delatnostBindingSource.DataSource = m_DelatnostiRepository.VratiSve().OrderBy(x => x.Naziv);
            UcitajVrsteDobavljaca();
        }

        private void UcitajVrsteDobavljaca()
        {
            if (IdDobavljaca.HasValue)
            {
                var vrsteDobavljaca = m_VrsteDobavljacaRepository.VratiZaDelatnost(IdDobavljaca.Value).OrderBy(x => x.Id);
                m_VrsteDobavljaca.Clear();
                foreach (var vrstaDobavljaca in vrsteDobavljaca)
                {
                    m_VrsteDobavljaca.Add(vrstaDobavljaca);
                }
            }
        }

        public override void OnNew()
        {
            if (IdDobavljaca.HasValue)
            {
                var vrstaDobavljaca = m_VrsteDobavljaca.AddNew();
                vrstaDobavljaca.IdDelatnosti = IdDobavljaca;
            }
        }

        public override void OnSave()
        {
            foreach (var vrstaDobavljaca in m_VrsteDobavljaca)
            {
                m_VrsteDobavljacaRepository.Save(vrstaDobavljaca);
            }
            OnRefresh();
        }

        public override void OnDelete()
        {
            var vd = gridView1.GetFocusedRow() as VrstaDobavljaca;
            if (vd != null)
            {
                m_VrsteDobavljacaRepository.Delete(vd.Id);
            }
            OnRefresh();
        }

        private int? IdDobavljaca
        {
            get
            {
                if (lookUpEdit1.EditValue != null && lookUpEdit1.EditValue != DBNull.Value)
                {
                    return (int)lookUpEdit1.EditValue;
                }
                return null;
            }
        }
    }
}
