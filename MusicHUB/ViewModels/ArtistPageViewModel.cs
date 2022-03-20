using Genius.Models.Artist;
using Genius.Models.Response;
using Genius.Models.Song;
using MusicHUB.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Services;
using Xamarin.Forms;
using Google.Apis.YouTube.v3;
using System.Windows.Input;
using MvvmHelpers.Commands;
using Java.Net;
using System;
using Xamarin.Essentials;
using System.Net.Http;
using Newtonsoft.Json;
using MusicHUB.Pages;
using System.Linq;

namespace MusicHUB.ViewModels
{
    public class ArtistPageViewModel : BindableObject
    {
        public ArtistPageViewModel(INavigation navigation, Artist response, Connections connections)
        {
            this.Response = response;
            this.Connections = connections;
            this.Navigation = navigation;
            ArtistInfo = new NotifyTaskCompletion<ArtistResponse>(Connections.GeniusClient.ArtistClient.GetArtist(Response.Id));
            ArtistsSongs = new NotifyTaskCompletion<ArtistsSongsResponse>(Connections.GeniusClient.ArtistClient.GetArtistsSongs(Response.Id, "popularity", "20"));
            ArtistsAlbums = new NotifyTaskCompletion<List<Genius.Models.Song.Album>>(GetAlbumsAsync(ArtistsSongs));
            ArtistVideos = new NotifyTaskCompletion<SearchListResponse>(GetEmbededVideos());
        }

        public Artist Response { get; set; }
        public Connections Connections { get; set; }
        public NotifyTaskCompletion<ArtistResponse> ArtistInfo { get => artistInfo; set { artistInfo = value; OnPropertyChanged(nameof(ArtistInfo)); } }
        public NotifyTaskCompletion<SearchListResponse> ArtistVideos { get; set; }
        public NotifyTaskCompletion<ArtistsSongsResponse> ArtistsSongs { get; set; }
        public NotifyTaskCompletion<List<Genius.Models.Song.Album>> ArtistsAlbums { get; set; }
        public ICommand CloseArtistPage { get => new Xamarin.Forms.Command(() => this.Navigation.PopAsync()); }
        private INavigation Navigation { get; set; }
        private NotifyTaskCompletion<ArtistResponse> artistInfo;

        private List<Album> GetAlbums(NotifyTaskCompletion<ArtistsSongsResponse> artistsSongs)
        {
            List<Album> Albums = new List<Album>();
            try
            {
                while (true)
                {
                    if (!artistsSongs.IsCompleted) continue;
                    foreach (var item in artistsSongs.Result.Response.Songs)
                    {
                        var Song = Connections.GeniusClient.SongClient.GetSong(item.Id).Result;
                        if (!Albums.Exists((album) => album.Id == Song?.Response.Song.Album.Id))
                        {
                            Albums.Add(Song?.Response.Song.Album);
                        }
                    }
                    return Albums;
                }
            }
            catch (Exception)
            {
                return Albums;
            }
        }

        private async Task<List<Album>> GetAlbumsAsync(NotifyTaskCompletion<ArtistsSongsResponse> artistsSongs)
        {
            return await Task.Run(() => GetAlbums(artistsSongs));
        }

        private async Task<SearchListResponse> GetEmbededVideos()
        {
            var service = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "AIzaSyAXKtHaocNan7-WeDF0PiQPCMs2H5Bb1Xo", });
            var reqest = service.Search.List("snippet");
            reqest.Q = Response.Name;
            reqest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
            reqest.MaxResults = 10;
            reqest.Type = "video";
            var responce = await reqest.ExecuteAsync();
            return responce;
        }

        public ICommand PlayYoutubeVideo
        {
            get => new AsyncCommand<SearchResult>(async (e) =>
               {
                   DependencyService.Get<IAudio>().Pause();
                   Uri uri = new Uri("https://www.youtube.com/watch?v=" + e.Id.VideoId);
                   await Browser.OpenAsync(uri);
                   e = null;
               });
        }

        public ICommand GoToFaceBook
        {
            get => new AsyncCommand(async () =>
            {
                Uri uri = new Uri($"https://www.facebook.com/{ArtistInfo.Result.Response.Artist.FacebookName}");
                await Browser.OpenAsync(uri);
            });
        }

        public ICommand GoToTwitter
        {
            get => new AsyncCommand(async () =>
            {
                Uri uri = new Uri($"https://twitter.com/{ArtistInfo.Result.Response.Artist.FacebookName}");
                await Browser.OpenAsync(uri);
            });
        }

        public ICommand GoToInstagram 
        {
            get => new AsyncCommand(async () =>
            {
                Uri uri = new Uri($"https://www.instagram.com/{ArtistInfo.Result.Response.Artist.TwitterName}/");
                await Browser.OpenAsync(uri);
            });
        }

        public ICommand SelectTrackCommand
        {
            get => new AsyncCommand<Song>(async (track) => await App.Current.MainPage.Navigation.PushAsync(new TrackPage(track.Id)));
        }
    }
}
