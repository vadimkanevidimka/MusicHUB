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
using MusicHUB.Droid;
using System.Threading;
using System.Windows.Input;
using MvvmHelpers.Commands;
using MusicHUB.Helpers.Sorters;

namespace MusicHUB.Pages
{
    public partial class MainPage : ContentPage
    {
        private IAudio Audio = DependencyService.Get<IAudio>();
        private ObservableCollection<Track> tracks { get; set; }
        private Connections Connections { get; set; }
        private Genius.GeniusClient GeniusClient { get; set; }
        private SortParams SorterParams { get; set; }
        public bool isUpdating { get; set; }
        MusicFilesCollector MusicFilesCollector = new MusicFilesCollector();
        public ObservableCollection<Track> Tracks { get => tracks; set { tracks = value; OnPropertyChanged(nameof(Tracks)); } }

        public MainPage(Connections connections)
        {
            InitializeComponent();
            BindingContext = this;
            isUpdating = false;
            Tracks = new ObservableCollection<Track>();
            SorterParams = new SortParams();
            this.Connections = connections;
            UpdateFileList();
        }

        protected override void OnAppearing()
        {

        }

        async void UpdateFileList()
        {
            isUpdating = true;
            Tracks.Clear();
            Tracks = new ObservableCollection<Track>(await MusicFilesCollector.GetTracks(new string[] { $"/storage/emulated/0/{Android.OS.Environment.DirectoryMusic}/", $"/storage/emulated/0/{Android.OS.Environment.DirectoryDownloads}/" }));
            isUpdating = false;
        }

        private void filesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Audio.SetQueue(Tracks.ToList());
            Track track = (Track)e.Item;
            AudioImgBottom.Source = track.ImageSource;
            Title.Text = track.Title;
            Author.Text = track.Artist;
            Audio.PlayAudioFile(track);
        }//НАжатие на элемент для проигрывания

        private void UnderPanelButton_Clicked(object sender, EventArgs e)
        {
        }//Нажатие на кнопку внизу

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            filesList.IsRefreshing = true;
            filesList.RefreshCommand = new Xamarin.Forms.Command(() => UpdateFileList());
            filesList.IsRefreshing = false;
        }

        private void AudioImgBottom_Clicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count == 1)
            {
                Navigation.PushModalAsync(new Player(Connections), true);
            }
        }

        private void PlayAllBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void Options_Clicked(object sender, EventArgs e)
        {
            ImageButton button = sender as ImageButton;
            ViewCell viewCell = button.Parent.Parent as ViewCell;
            Navigation.PushPopupAsync(new PopUpContextActionsOnTrack((Track)viewCell.BindingContext, ViewModels.ContextActions.TrackContextActionState.AtList));
        }

        public ICommand Sort
        {
            get => new Xamarin.Forms.Command(() => {
                App.Current.MainPage.Navigation.PushModalAsync(new SortPopupPage(SorterParams,
                    async () =>
                    {
                        if (SorterParams is null) return;
                        Tracks = new ObservableCollection<Track>(await SorterParams.Sorter.SortAsync(Tracks, SorterParams.OrderType));
                    }));
            });
        }
    }
}
