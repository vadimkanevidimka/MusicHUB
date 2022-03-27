using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading.Forms.Platform;
using MediaManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHUB.Droid
{
    [Activity(Label = "MusicHUB", Icon = "@mipmap/MusicHUB",  MainLauncher = true, Theme = "@style/MyTheme.Splash", NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            RequestedOrientation = ScreenOrientation.Portrait;
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.ForceNotFullscreen);
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await SimulateStartup();
        }

        private async Task SimulateStartup()
        {
            await Task.Delay(1000);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}