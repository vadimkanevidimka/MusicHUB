﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Genius;
using MusicHUB.DataBaseServices;
using MusicHUB.DependencyInjection;
using MusicHUB.Interfaces;
using SQLite;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MusicHUB.Droid
{
    [Activity(Label = "MusicHUB", Icon = "@mipmap/MusicHUB", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
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
            GrantPermissions(new string[] { "android.permission.READ_EXTERNAL_STORAGE", "android.permission.WRITE_EXTERNAL_STORAGE" }, 1);
            var DataBase = await ConnectionCreator.Init();
            Genius.GeniusClient geniusClient = new GeniusClient("AkUpkfIcZocLUFBxkZT8kUUb4pUxiHjo7ioK7eGPdsjy3TtE596RAN5iQZIH9G1B");
            Connections connections = new Connections(new DataBaseService(DataBase), geniusClient);
            LoadApplication(new App(connections));
        }

        private void GrantPermissions(string[] permissions,int requestCode)
        {
            foreach (var item in permissions)
            {
                var rezult = base.CheckSelfPermission(item);
                if (base.CheckSelfPermission(item) == Permission.Denied)
                {
                    base.RequestPermissions(permissions, requestCode);
                    return;
                }
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