using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.Models
{
    [Table("AlbumsTracks")]
    class AlbumsTracks
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public int TrackId { get; set; }
    }
}
