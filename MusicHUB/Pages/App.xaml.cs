using MusicHUB.DependencyInjection;
using MusicHUB.Pages;
using Xamarin.Forms;

namespace MusicHUB
{
    public partial class App : Application
    {
        public App(Connections connections)
        {
            Device.SetFlags(new string[] { "AppTheme_Experimental" });
            InitializeComponent();
            MainPage = new MainTabbedPage(connections);
        }

        public Connections Connections { get; set; }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
