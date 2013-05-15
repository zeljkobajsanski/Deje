using System;
using System.Drawing;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    public class ArtikliViewFactory : IViewFactory
    {
        public ArtikliViewFactory()
        {
            Id = Guid.NewGuid();
            MenuCaption = "Artikli po kategoriji";
        }

        public IView Create()
        {
            return new Artikli {Id = Id, Caption = MenuCaption};
        }

        public Guid Id { get; private set; }
        public string MenuCaption { get; private set; }
        public Image NavigationIcon { get; private set; }
    }
}