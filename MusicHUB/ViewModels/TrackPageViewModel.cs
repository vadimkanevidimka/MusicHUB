using Genius.Models.Response;
using Genius.Models.Song;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MusicHUB.ViewModels
{
    class TrackPageViewModel : BindableObject
    {
        private NotifyTaskCompletion<SongResponse> song;
        public NotifyTaskCompletion<SongResponse> Song { get => song; set { song = value; OnPropertyChanged(nameof(Song)); } }
        
        private NotifyTaskCompletion<Video> video;
        public NotifyTaskCompletion<Video> Video { get => video; set { video = value; OnPropertyChanged(nameof(Video)); } }
        private string Artist { get; set; }
        private string TrackTitle { get; set; }

        public TrackPageViewModel(ulong id, string artist = null, string title = null)
        {
            Artist = artist;
            TrackTitle = title;
            GetSong(id);
        }
         
        private void GetSong(ulong id)
        {
            if (id != 0)
            {
                this.Song = new NotifyTaskCompletion<SongResponse>(App.Connections.GeniusClient.SongClient.GetSong(id));
            }
            else
            {
                this.Song = new NotifyTaskCompletion<SongResponse>(GetSongBytitleAndArtist(Artist, TrackTitle));
            }

            Video = new NotifyTaskCompletion<Video>(GetVideo());
        }

        private async Task<SongResponse> GetSongBytitleAndArtist(string artist, string tracktitle)
        {
            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(tracktitle))
            {
                return null;
            }

            var search = await App.Connections.GeniusClient.SearchClient.Search($"{tracktitle} by {artist}");
            var song = search.Response.Hits.Where((c) => c.Result.PrimaryArtist.Name.Contains(artist, StringComparison.CurrentCultureIgnoreCase)).First();
            return await App.Connections.GeniusClient.SongClient.GetSong(song.Result.Id);
        }

        private async Task<Video> GetVideo()
        {
            while (true)
            {
                if (Song.Result == null) continue;
                else
                {
                    YouTubeService youTubeService = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "AIzaSyAXKtHaocNan7-WeDF0PiQPCMs2H5Bb1Xo", });
                    var request = youTubeService.Videos.List("snippets");
                    string videoURL = Song?.Result?.Response.Song.Media.Where((c) => c.Provider == "youtube" && c.Type == "video").First().Url;
                    request.Id = videoURL[videoURL.IndexOf("watch?v=")..];
                    var responce = await request.ExecuteAsync();
                    return responce.Items[0];
                }
            }
        }
    }
}
