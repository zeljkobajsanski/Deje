using System;
using System.Drawing;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    public class SinonimiViewFactory2 : IViewFactory
    {
        public SinonimiViewFactory2()
        {
            Id = Guid.NewGuid();
            MenuCaption = "Sinonimi";
        }

        public IView Create()
        {
            return new Sinonimi2 {Id = Id, Caption = MenuCaption};
        }

        public Guid Id { get; private set; }
        public string MenuCaption { get; private set; }
        public Image NavigationIcon { get; private set; }
    }
}