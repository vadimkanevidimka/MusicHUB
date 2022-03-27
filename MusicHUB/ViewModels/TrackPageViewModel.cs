using Genius.Models.Artist;
using Genius.Models.Response;
using Genius.Models.Song;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using MusicHUB.Pages;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
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

        private List<Artist> artists;
        public List<Artist> Artists { get => artists; set { artists = value; OnPropertyChanged(nameof(Artists)); } }

        public TrackPageViewModel(ulong id, string artist = null, string title = null)
        {
            Artist = artist;
            TrackTitle = title;
            GetSong(id);
            Song.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Result")
                {
                    artists = new List<Artist>();
                    artists.AddRange(Song.Result.Response.Song.FeaturedArtists.Append(Song.Result.Response.Song.PrimaryArtist));
                    OnPropertyChanged(nameof(Artists));
                    Video = new NotifyTaskCompletion<Video>(GetVideo());
                }
            };
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
            YouTubeService youTubeService = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "AIzaSyAXKtHaocNan7-WeDF0PiQPCMs2H5Bb1Xo", });
            var request = youTubeService.Videos.List("snippet");
            string videoURL = Song?.Result?.Response.Song.Media.Where((c) => c.Provider == "youtube" && c.Type == "video").First().Url;
            request.Id = videoURL[(videoURL.IndexOf("watch?v=") + "watch?v=".Length)..];
            var responce = await request.ExecuteAsync();
            return responce.Items[0];
        }

        public ICommand YoutubeVideoCommand 
        {
            get => new AsyncCommand<Video>(async (vidos) =>
            {
                DependencyService.Get<IAudio>().Pause();
                Uri uri = new Uri("https://www.youtube.com/watch?v=" + video.Result.Id);
                await Browser.OpenAsync(uri);
                vidos = null;
            });
        }

        public ICommand ArtistClickCommand
        {
            get => new AsyncCommand<Artist>(async (artist) =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {

                    await App.Current.MainPage.Navigation.PushAsync(new ArtistPage(artist));
                }
                else
                {
                    App.Message.SetText("Отсутствует подключение к интернету");
                    App.Message.Duration = 0;
                    App.Message.Show();
                }
            });
        }
    }
}
