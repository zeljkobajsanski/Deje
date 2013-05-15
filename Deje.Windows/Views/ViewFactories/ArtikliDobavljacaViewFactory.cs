using System;
using System.Drawing;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    public class ArtikliDobavljacaViewFactory : IViewFactory
    {
        public ArtikliDobavljacaViewFactory()
        {
            Id = Guid.NewGuid();
            MenuCaption = "Artikli dobavljaèa";
        }

        public IView Create()
        {
            return new ArtikliDobavljaca {Id = Id, Caption = MenuCaption};
        }

        public Guid Id { get; private set; }
        public string MenuCaption { get; private set; }
        public Image NavigationIcon { get; private set; }
    }
}