using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MusicHUB.Droid.Mocks;
using MusicHUB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: Dependency(typeof(AndroidSystemService))]
namespace MusicHUB.Droid.Mocks
{
    class AndroidSystemService : IAndroidSystem
    {
        public RingtoneManager RingtoneManager => throw new NotImplementedException();
    }
}