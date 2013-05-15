using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Deje.Windows.Views;
using Deje.Windows.Views.ViewFactories;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Shell;

namespace Deje.Windows
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                try
                {
                    configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
                }
                catch (Exception)
                {
                    // for a console app, reading from App.config
                    configSetter(System.Configuration.ConfigurationManager.AppSettings[configName]);
                }
            });
            //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Application.ThreadException += OnThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");

            var shell = new Shell.Shell {ApplicationName = "Deje"};

            var dobavljaci = new NavGroup("Dobavljači", null);
            dobavljaci.AddViewFactory(new StatusiDobavljacaViewFactory());
            dobavljaci.AddViewFactory(new DelatnostiViewFactory());
            dobavljaci.AddViewFactory(new VrsteDobavljacaViewFactory());
            dobavljaci.AddViewFactory(new DobavljaciViewFactory());
            shell.AddGroup(dobavljaci);

            var artikli = new NavGroup("Artikli", null);
            artikli.AddViewFactory(new KategorijeArtikalaViewFactory());
            artikli.AddViewFactory(new UpravljanjeKategorijamaArtikalaViewFactory());
            artikli.AddViewFactory(new ImportArtikalaViewFactory());
            artikli.AddViewFactory(new ArtikliDobavljacaViewFactory());
            artikli.AddViewFactory(new ArtikliViewFactory());
            artikli.AddViewFactory(new SlicnostArtikalaViewFactory());
            artikli.AddViewFactory(new SinonimiViewFactory());
            artikli.AddViewFactory(new SinonimiViewFactory2());
            shell.AddGroup(artikli);

            var tools = new NavGroup("Alati", null);
            tools.AddViewFactory(new StressTestViewFactory());
            shell.AddGroup(tools);

            

            Application.Run(shell);
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            XtraMessageBox.Show(e.Exception.Message, "Deje.Windows", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}