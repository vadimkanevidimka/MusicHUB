using MusicHUB.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MusicHUB.DataBaseServices
{
    public static class ConnectionCreator
    {
        public static async Task<SQLiteAsyncConnection> Init()
        {
            var dataBasePath = Path.Combine(FileSystem.AppDataDirectory, "MusicHub.db");
            SQLiteAsyncConnection DataBase = new SQLiteAsyncConnection(dataBasePath);
            await DataBase.CreateTableAsync<DbTrack>();
            return DataBase;
        }
    }
}
