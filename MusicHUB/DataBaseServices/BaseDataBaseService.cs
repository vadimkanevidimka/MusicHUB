using MusicHUB.Interfaces;
using MusicHUB.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.DataBaseServices
{
    public abstract class BaseDataBaseService : IDataBaseService
    {
        protected SQLiteAsyncConnection DataBase;

        public BaseDataBaseService(SQLiteAsyncConnection datatbase)
        {
            this.DataBase = datatbase;
        }

        SQLiteAsyncConnection IDataBaseService.DataBase => throw new NotImplementedException();
    }
}
