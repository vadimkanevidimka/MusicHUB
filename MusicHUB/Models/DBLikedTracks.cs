using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.Models
{
    [Table("LikedTracks")]
    class DBLikedTracks
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public int TrackId { get; set; }
        //public ImageSource AlbumCover { get; set; }
    }
}
