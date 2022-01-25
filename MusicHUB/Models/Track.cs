using MusicHUB.Interfaces;
using SQLite;
using Xamarin.Forms;

namespace MusicHUB.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string Uri { get; set; }
        public int Duration { get; set; }
        public string DurationString { get; set; }
        public ImageSource ImageSource { get => DependencyService.Get<IImage>().GetTrackPic(this.Uri);}
        public ImageSource LowResolutionImage { get => DependencyService.Get<IImage>().GetLowerResImage(this.Uri); }
    }

    public class DbTrack
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Path { get; set; }
    }
}
