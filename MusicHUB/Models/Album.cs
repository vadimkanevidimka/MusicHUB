using MusicHUB.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.Models
{
    [Table("Albums")]
    public class Album
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public byte[] Image { get; set; }
        public string AlbumName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
