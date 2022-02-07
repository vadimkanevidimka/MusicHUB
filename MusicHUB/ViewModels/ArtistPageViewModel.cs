using Genius.Models.Artist;
using Genius.Models.Response;
using Genius.Models.Song;
using MusicHUB.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MusicHUB.ViewModels
{
    public class ArtistPageViewModel
    {
        public ArtistPageViewModel(INavigation navigation, Artist response, Connections connections)
        {
            this.Response = response;
            this.Connections = connections;
            this.Navigation = navigation;
            ArtistInfo = new NotifyTaskCompletion<ArtistResponse>(Connections.GeniusClient.ArtistClient.GetArtist(Response.Id));
            ArtistsSongs = new NotifyTaskCompletion<ArtistsSongsResponse>(Connections.GeniusClient.ArtistClient.GetArtistsSongs(Response.Id, "popularity", "10"));
            ArtistsAlbums = new NotifyTaskCompletion<List<Genius.Models.Song.Album>>(GetAlbumsAsync(ArtistsSongs));
        }

        public Artist Response { get; set; }
        public Connections Connections { get; set; }
        public NotifyTaskCompletion<ArtistResponse> ArtistInfo { get; set; }
        public NotifyTaskCompletion<List<Genius.Models.Song.Album>> ArtistsAlbums { get; set; }
        public NotifyTaskCompletion<ArtistsSongsResponse> ArtistsSongs { get; set; }
        public Command CloseArtistPage { get => new Command(() => this.Navigation.PopAsync()); }
        private INavigation Navigation { get; set; }

        private List<Album> GetAlbums(NotifyTaskCompletion<ArtistsSongsResponse> artistsSongs)
        {
            List<Album> Albums = new List<Album>();
            while (true)
            {
                if (!artistsSongs.IsCompleted) continue;
                foreach (var item in artistsSongs.Result.Response.Songs)
                {
                    var Song = Connections.GeniusClient.SongClient.GetSong(item.Id).Result;
                    if (!Albums.Exists((album) => album.Id == Song.Response.Song.Album.Id))
                    {
                        Albums.Add(Song.Response.Song.Album);
                    }
                }
                return Albums;
            }
        }

        private async Task<List<Album>> GetAlbumsAsync(NotifyTaskCompletion<ArtistsSongsResponse> artistsSongs)
        {
            return await Task.Run(() => GetAlbums(artistsSongs));
        }
    }
}
