using System;
using System.Drawing;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    public class SlicnostArtikalaViewFactory : IViewFactory
    {
        public SlicnostArtikalaViewFactory()
        {
            Id = Guid.NewGuid();
            MenuCaption = "Artikli po nazivu";
        }

        public IView Create()
        {
            return new SlicnostArtikala {Id = Id, Caption = MenuCaption};
        }

        public Guid Id { get; private set; }
        public string MenuCaption { get; private set; }
        public Image NavigationIcon { get; private set; }
    }
}