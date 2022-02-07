using MusicHUB.DependencyInjection;
using MusicHUB.Pages;
using SQLite;
using Xamarin.Forms;

namespace MusicHUB
{
    public partial class App : Application
    {
        public App(Connections connections)
        {
            Connections = connections;
            Device.SetFlags(new string[] { "AppTheme_Experimental" });
            InitializeComponent();
            MainPage = new NavigationPage(new MainTabbedPage(connections)) { Title = "Главная", BarBackgroundColor = Color.Wheat};
        }

        public static Connections Connections { get; set; }

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
