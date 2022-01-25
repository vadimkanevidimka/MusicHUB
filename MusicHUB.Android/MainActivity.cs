using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MusicHUB.DataBaseServices;
using MusicHUB.Interfaces;
using SQLite;
using System.IO;
using Xamarin.Essentials;

namespace MusicHUB.Droid
{
    [Activity(Label = "MusicHUB", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            RequestedOrientation = ScreenOrientation.Portrait;
            Rg.Plugins.Popup.Popup.Init(this);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 27, 27, 27));
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.ForceNotFullscreen);
            RequestPermissions(new string[] { "android.permission.READ_EXTERNAL_STORAGE" }, 0);
            var DataBase = await ConnectionCreator.Init();
            LoadApplication(new App(DataBase));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}