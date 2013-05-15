using System;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    class UpravljanjeKategorijamaArtikalaViewFactory : IViewFactory
    {

        public IView Create()
        {
            return new UpravljanjeKategorijamaArtikala { Caption = MenuCaption, Id = Id };
        }

        public Guid Id
        {
            get { return Guid.NewGuid(); }
        }

        public string MenuCaption
        {
            get { return "Upravljanje kategorijama artikala"; }
        }

        public System.Drawing.Image NavigationIcon
        {
            get { return null; }
        }
    }
}
