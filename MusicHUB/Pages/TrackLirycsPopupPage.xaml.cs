using MusicHUB.Models;
using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackLirycsPopupPage : ContentPage
    {
        public TrackLirycsPopupPage()
        {
            InitializeComponent();
            this.BackgroundColor = new Color(0, 0, 0, 0.4);
            BindingContext = this;
            Lyrics = new NotifyTaskCompletion<string>(GetLirycs());
        }

        private NotifyTaskCompletion<string> lyrics;
        public NotifyTaskCompletion<string> Lyrics { get => lyrics; set { lyrics = value; OnPropertyChanged(nameof(Lyrics)); } }

        public ICommand ClosePopupPage
        {
            get => new AsyncCommand(async () => await App.Current.MainPage.Navigation.PopModalAsync());
        }

        private async Task<string> GetLirycs()
        {
            try
            {
                var track = DependencyService.Get<IAudio>().GetCurrentTrack();
                var rezult = await App.Connections.GeniusClient.SearchClient.Search($"{track.Title} by {track.Artist}");
                var song = await App.Connections.GeniusClient.SongClient.GetSong(rezult.Response.Hits.Where((s) => s.Result.Stats.PageViews == rezult.Response.Hits.Max((c) => c.Result.Stats.PageViews) || s.Result.Title.Contains(track.Title, StringComparison.CurrentCultureIgnoreCase)).First().Result.Id);
                return song.Response.Song.Url;
            }
            catch (Exception ex)//<div class="lyrics">
            {
                return "ничего не найдено";
            }
        }
    }
}