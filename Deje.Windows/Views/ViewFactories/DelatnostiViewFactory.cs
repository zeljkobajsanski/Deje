using System;
using System.Drawing;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    public class DelatnostiViewFactory : IViewFactory
    {
        public DelatnostiViewFactory()
        {
            Id = Guid.NewGuid();
            MenuCaption = "Delatnosti";
        }

        public IView Create()
        {
            return new Delatnosti {Caption = MenuCaption, Id = Id};
        }

        public Guid Id { get; private set; }
        public string MenuCaption { get; private set; }
        public Image NavigationIcon { get; private set; }
    }
}