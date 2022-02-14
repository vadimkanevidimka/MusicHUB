using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MusicHUB.DataBaseServices;
using MusicHUB.Droid.Mocks;
using MusicHUB.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(MusicHUB.Droid.Mocks.DataBaseService))]
namespace MusicHUB.Droid.Mocks
{
    class DataBaseService : IDataBaseService
    {
        public SQLiteAsyncConnection DataBase => Base;

        private SQLiteAsyncConnection Base;

        public DataBaseService()
        {
            Iniit();
        }

        private async void Iniit()
        {
            Base = await ConnectionCreator.Init();
        }
    }

        
}