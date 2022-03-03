using System;
using System.Collections.Generic;
using System.Text;


namespace MusicHUB.Models
{
    [Serializable]
    class LirycsResponse
    {
        public int TrackId { get; set; }
        public int LyricId { get; set; }
        public string LyricSong { get; set; }
        public string LyricArtist { get; set; }
        public string LyricUrl { get; set; }
        public int LyricRank { get; set; }
        public string LyricCorrectUrl { get; set; }
        public string Lyric { get; set; }
    }
}
