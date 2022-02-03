using Genius.Clients.Interfaces;
using Genius.Models.Artist;
using Genius.Models.Response;
using MusicHUB.DependencyInjection;
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
        }

        public Artist Response { get; set; }
        public Connections Connections { get; set; }
        public NotifyTaskCompletion<ArtistResponse> ArtistInfo { get; set; }
        public NotifyTaskCompletion<ArtistsSongsResponse> ArtistsSongs { get; set; }
        public Command CloseArtistPage { get => new Command(() => this.Navigation.PopAsync()); }
        private INavigation Navigation { get; set; }
    }
}
