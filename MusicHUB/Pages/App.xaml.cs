using MusicHUB.DataBaseServices;
using MusicHUB.Interfaces;
using MusicHUB.Pages;
using SQLite;
using Xamarin.Forms;

namespace MusicHUB
{
    public partial class App : Application
    {
        public App(SQLiteAsyncConnection Database)
        {
            Device.SetFlags(new string[] { "AppTheme_Experimental" });
            InitializeComponent();
            MainPage = new MainTabbedPage(new DataBaseService(Database));
        }

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
