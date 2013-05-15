using System;
using System.Drawing;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    public class KategorijeArtikalaViewFactory : IViewFactory
    {
        public KategorijeArtikalaViewFactory()
        {
            Id = Guid.NewGuid();
            MenuCaption = "Kategorije artikala";
        }

        public IView Create()
        {
            return new KategorijeArtikala {Id = Id, Caption = MenuCaption};
        }

        public Guid Id { get; private set; }
        public string MenuCaption { get; private set; }
        public Image NavigationIcon { get; private set; }
    }
}