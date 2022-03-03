using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.Models
{
    [Table("RecentlyPlayed")]
    public class RecentlyPlayed
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TrackId { get; set; }
        public DateTime ListenTime { get; set; }
    }
}
