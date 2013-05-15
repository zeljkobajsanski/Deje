using System;
using System.Drawing;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    public class StatusiDobavljacaViewFactory : IViewFactory
    {
        public StatusiDobavljacaViewFactory()
        {
            Id = Guid.NewGuid();
            MenuCaption = "Statusi dobavljaèa";
        }

        public IView Create()
        {
            return new StatusiDobavljaca {Id = Id, Caption = MenuCaption};
        }

        public Guid Id { get; private set; }
        public string MenuCaption { get; private set; }
        public Image NavigationIcon { get; private set; }
    }
}