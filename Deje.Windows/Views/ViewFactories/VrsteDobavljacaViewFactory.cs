using System;
using System.Drawing;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    public class VrsteDobavljacaViewFactory : IViewFactory
    {
        public VrsteDobavljacaViewFactory()
        {
            Id = Guid.NewGuid();
            MenuCaption = "Vrste dobavljaèa";
        }

        public IView Create()
        {
            return new VrsteDobavljaca {Id = Id, Caption = MenuCaption};
        }

        public Guid Id { get; private set; }
        public string MenuCaption { get; private set; }
        public Image NavigationIcon { get; private set; }
    }
}