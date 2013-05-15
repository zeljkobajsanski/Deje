using System;
using System.Drawing;
using Shell;

namespace Deje.Windows.Views.ViewFactories
{
    public class StressTestViewFactory : IViewFactory
    {
        public StressTestViewFactory()
        {
            Id = Guid.NewGuid();
            MenuCaption = "Stres test";
        }

        public IView Create()
        {
            return new StressTest {Id = Id, Caption = MenuCaption};
        }

        public Guid Id { get; private set; }
        public string MenuCaption { get; private set; }
        public Image NavigationIcon { get; private set; }
    }
}