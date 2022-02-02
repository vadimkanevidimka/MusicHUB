using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using System.Collections.ObjectModel;
using MusicHUB.Interfaces;
using MusicHUB.DependencyInjection;

namespace MusicHUB.Pages
{
    public partial class MainPage : ContentPage
    {
        private IAudio Audio = DependencyService.Get<IAudio>();
        private Connections Connections { get; set; }
        private Genius.GeniusClient GeniusClient { get; set; }
        public ObservableCollection<Track> Tracks { get => Audio.Tracks; }

        public MainPage(Connections connections)
        {
            InitializeComponent();
            BindingContext = this;
            this.Connections = connections;
        }

        protected override void OnAppearing()
        {
            Track track = Audio.GetCurrentTrack();
            UnderPanel.IsVisible = true;
            AudioImgBottom.Source = track.ImageSource;
            Title.Text = track.Title;
            Author.Text = track.Artist;
        }

        void UpdateFileList()
        {

        }//Обновление плейлиста

        private void filesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Track track = (Track)e.Item;
            AudioImgBottom.Source = track.ImageSource;
            Title.Text = track.Title;
            Author.Text = track.Artist;
            Audio.PlayAudioFile(track);
            ((ListView)sender).SelectedItem = null;
        }//НАжатие на элемент для проигрывания

        private void UnderPanelButton_Clicked(object sender, EventArgs e)
        {
        }//Нажатие на кнопку внизу

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            filesList.IsRefreshing = true;
            filesList.RefreshCommand = new Command(() => UpdateFileList());
            filesList.IsRefreshing = false;
        }

        private void AudioImgBottom_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Player(Connections), true);
        }

        private void PlayAllBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void Options_Clicked(object sender, EventArgs e)
        {
            ImageButton button = sender as ImageButton;
            ViewCell viewCell = button.Parent.Parent as ViewCell;
            Navigation.PushPopupAsync(new PopUpPageFromBottom((Track)viewCell.BindingContext));
        }
    }
}
