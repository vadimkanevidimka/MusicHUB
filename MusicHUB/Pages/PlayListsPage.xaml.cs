using MusicHUB.DependencyInjection;
using MusicHUB.Interfaces;
using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayListsPage : ContentPage
    {
        private Connections Connections { get; set; }
        private IAudio Audio = DependencyService.Get<IAudio>();
        public NotifyTaskCompletion<IList<Track>> Items { get; set; }

        public PlayListsPage(Connections connections)
        {
            this.Connections = connections;
            Items = new NotifyTaskCompletion<IList<Track>>(Gettracks());
            InitializeComponent();
        }

        private async Task<IList<Track>> Gettracks()
        {
            var liked = Connections.BaseDataBaseService.DataBase.Table<DBLikedTracks>().ToListAsync();
            return await Connections.BaseDataBaseService.DataBase.Table<Track>().ToListAsync();
        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
