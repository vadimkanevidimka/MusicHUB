using Android.App;
using Android.Content;
using Android.Media;
using MusicHUB.Droid.Mocks;
using MusicHUB.Interfaces;
using Android.Provider;
using System;
using Xamarin.Forms;
using System.IO;

[assembly: Dependency(typeof(AndroidSystemService))]
namespace MusicHUB.Droid.Mocks
{
    class AndroidSystemService : IAndroidSystem
    {
        public Context AppContext => Android.App.Application.Context;

        public void SetAsRingtone(string url)
        {
            if (string.IsNullOrEmpty(url)) return;
            if (!Settings.System.CanWrite(AppContext))
            {
                Intent intent = new Intent(Settings.ActionManageWriteSettings);
                intent.SetData(Android.Net.Uri.Parse($"package:{Android.App.Application.Context.PackageName}"));
                intent.AddFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
            }
            var newringtonefile = $"/storage/emulated/0/Ringtones/{url.Substring(url.LastIndexOf('/'))}";
            File.Copy(url, newringtonefile);
            Android.Net.Uri uri = new Android.Net.Uri.Builder().Path(newringtonefile).Build();
            RingtoneManager.SetActualDefaultRingtoneUri(AppContext, RingtoneType.Ringtone, uri);
        }


    }
}