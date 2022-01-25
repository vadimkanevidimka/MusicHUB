using MusicHUB.Interfaces;
using MusicHUB.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayListsPage : ContentPage
    {
        private IDataBaseService Database;
        private IAudio Audio = DependencyService.Get<IAudio>();
        public ObservableCollection<Track> Items;

        public PlayListsPage(IDataBaseService dataBaseService)
        {
            this.Database = dataBaseService;
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
