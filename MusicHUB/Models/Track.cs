using MusicHUB.Interfaces;
using SQLite;
using Xamarin.Forms;

namespace MusicHUB.Models
{
    [Table("Tracks")]
    public class Track
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public int Bitrate { get; set; }
        [Unique]
        public string Uri { get; set; }
        public int Duration { get; set; }
        public string DurationString { get; set; }
        [Ignore]
        public ImageSource ImageSource { get => DependencyService.Get<IImage>().GetTrackPic(this.Uri);}
        [Ignore]
        public ImageSource LowResolutionImage { get => DependencyService.Get<IImage>().GetLowerResImage(this.Uri); }
        [Ignore]
        public int UniquePosition { get; set; }

        public Track MemberWiseClone()
        {
            return (Track)base.MemberwiseClone();
        }
    }
}
