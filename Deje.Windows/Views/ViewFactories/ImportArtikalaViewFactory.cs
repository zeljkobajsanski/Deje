using System;
using System.Drawing;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    public class ImportArtikalaViewFactory : IViewFactory
    {
        public ImportArtikalaViewFactory()
        {
            Id = Guid.NewGuid();
            MenuCaption = "Import artikala";
        }

        public IView Create()
        {
            return new ImportArtikala {Id = Id, Caption = MenuCaption};
        }

        public Guid Id { get; private set; }
        public string MenuCaption { get; private set; }
        public Image NavigationIcon { get; private set; }
    }
}