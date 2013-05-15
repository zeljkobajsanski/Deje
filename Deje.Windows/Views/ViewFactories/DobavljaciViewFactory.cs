using System;
using System.Drawing;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    public class DobavljaciViewFactory : IViewFactory
    {
        public IView Create()
        {
            return new Dobavljaci() {Caption = MenuCaption, Id = Id};
        }

        public Guid Id { get { return Guid.NewGuid(); } }
        public string MenuCaption { get { return "Dobavljaèi"; } }
        public Image NavigationIcon { get { return null; } }
    }
}