using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using FFImageLoading.Forms.Platform;
using Genius;
using MediaManager;
using MusicHUB.DataBaseServices;
using MusicHUB.DependencyInjection;
using MusicHUB.Interfaces;
using SQLite;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MusicHUB.Droid
{
    [Activity(Label = "MusicHUB", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            CrossMediaManager.Current.Init(this);
            CachedImageRenderer.Init(true);
            RequestedOrientation = ScreenOrientation.Portrait;
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.ForceNotFullscreen);
            GrantPermissions(new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage, Manifest.Permission.AccessMediaLocation, Manifest.Permission.ManageExternalStorage, Manifest.Permission.MediaContentControl});
            var DataBase = await ConnectionCreator.Init();
            Genius.GeniusClient geniusClient = new GeniusClient("AkUpkfIcZocLUFBxkZT8kUUb4pUxiHjo7ioK7eGPdsjy3TtE596RAN5iQZIH9G1B");
            Connections connections = new Connections(new DataBaseService(DataBase), geniusClient);
            LoadApplication(new App(connections));
        }

        private async void GrantPermissions(string[] permissions)
        {
            foreach (var permission in permissions)
            {
                if (CheckSelfPermission(permission) == Permission.Denied)
                {
                    RequestPermissions(new string[] { permission }, 1);
                }
            }

            var StorrageRead = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (StorrageRead == PermissionStatus.Denied)
            {
                await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            var StorrageWrite = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (StorrageWrite == PermissionStatus.Denied)
            {
                await Permissions.RequestAsync<Permissions.StorageWrite>();
            }

            var media = await Permissions.CheckStatusAsync<Permissions.Media>();
            if (media == PermissionStatus.Denied)
            {
                await Permissions.RequestAsync<Permissions.Media>();
            }
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : Permissions.BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }
            return (PermissionStatus)status;
        }
    }
}