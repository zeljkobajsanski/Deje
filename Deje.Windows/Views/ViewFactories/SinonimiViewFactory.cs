using System;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    class SinonimiViewFactory : IViewFactory
    {
        public IView Create()
        {
            return new Sinonimi() { Id = Id, Caption = MenuCaption };
        }

        public Guid Id
        {
            get { return Guid.NewGuid(); }
        }

        public string MenuCaption
        {
            get { return "Sinonimi artikala"; }
        }

        public System.Drawing.Image NavigationIcon
        {
            get { return null; }
        }
    }
}
