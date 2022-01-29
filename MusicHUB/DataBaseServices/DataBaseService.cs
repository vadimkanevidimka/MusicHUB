﻿using MusicHUB.Interfaces;
using MusicHUB.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.DataBaseServices
{
    public class DataBaseService : BaseDataBaseService
    {
        public DataBaseService(SQLiteAsyncConnection database) 
            : base(database)
        {

        }
    }
}