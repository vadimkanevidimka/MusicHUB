using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.Models
{
    public class Album
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string AlbumName { get; set; }
        public List<int> TracksId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
