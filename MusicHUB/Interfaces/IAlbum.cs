using MusicHUB.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.Interfaces
{
    interface IAlbum
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public byte[] Image { get; set; }
    }
}
