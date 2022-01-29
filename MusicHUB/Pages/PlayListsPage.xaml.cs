using MusicHUB.DependencyInjection;
using MusicHUB.Interfaces;
using MusicHUB.Models;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayListsPage : ContentPage
    {
        private Connections Connections { get; set; }
        private IAudio Audio = DependencyService.Get<IAudio>();
        public ObservableCollection<Track> Items;

        public PlayListsPage(Connections connections)
        {
            this.Connections = connections;
            InitializeComponent();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
