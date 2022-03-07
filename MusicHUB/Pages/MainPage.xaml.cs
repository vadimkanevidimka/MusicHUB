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
using MusicHUB.ViewModels;
using MusicHUB.EventArgumentss;

namespace MusicHUB.Pages
{
    public partial class MainPage : ContentPage
    {
        private IAudio Audio = DependencyService.Get<IAudio>();
        private ObservableCollection<Track> tracks { get; set; }
        private Connections Connections { get; set; }
        private Genius.GeniusClient GeniusClient { get; set; }
        private SortParams SorterParams { get; set; }
        private Track currentTrack;
        private bool isupdating;
        public Track CurrentTrack { get => currentTrack; set { currentTrack = value; OnPropertyChanged(nameof(CurrentTrack)); } }
        public bool isUpdating { get => isupdating; set { isupdating = value; OnPropertyChanged(nameof(isUpdating)); } }
        private MusicFilesCollector MusicFilesCollector = new MusicFilesCollector();
        public ObservableCollection<Track> Tracks { get => tracks; set { tracks = value; OnPropertyChanged(nameof(Tracks)); } }
        private ResourceClassPlayerPage resourceClass { get; set; }
        public ResourceClassPlayerPage ResourceClass { get => resourceClass; set { resourceClass = value; OnPropertyChanged(nameof(ResourceClass)); } }
        private float progress = 0;
        public float Progress { get => progress; set { progress = value; OnPropertyChanged(nameof(Progress)); } }
       

        public MainPage(Connections connections)
        {
            InitializeComponent();
            BindingContext = this;
            isUpdating = false;
            ResourceClass = new ResourceClassPlayerPage();
            Tracks = new ObservableCollection<Track>();
            SorterParams = new SortParams();
            this.Connections = connections;
            UpdateFileList();
            Audio.OntrackChanged += SetBottomPanelData;
            Audio.OnPlayerTimeChanged += SetTimeStamp;
            Audio.OnstateChanged += stateChanged;
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
            Audio.PlayAudioFile(track);
        }//НАжатие на элемент для проигрывания

        private void SetBottomPanelData(object sender, EventArgs e)
        {
            CurrentTrack = Audio.GetCurrentTrack();
            ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg;
        }

        private void stateChanged(object sender, EventArgs e)
        {
            ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg;
            OnPropertyChanged(nameof(ResourceClass));
        }

        private void SetTimeStamp(PlayerTimeEventArgs e)
        {
            if(e.Duration > 0) Progress = (float)e.CurrentTime / (float)e.Duration;
        }

        private void AudioImgBottom_Clicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count == 1)
            {
                Navigation.PushModalAsync(new Player(Connections), true);
            }
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

        public ICommand RefreshCommand
        {
            get => new Xamarin.Forms.Command(() => UpdateFileList());
        }

        public ICommand ShuffleCommand
        {
            get => new AsyncCommand(async () =>
            {
                Audio.SetQueue(Tracks.ToList());
                await Audio.Shuffle();
                Audio.PlayAudioFile(Audio.Tracks[0]);
            });
        }

        public ICommand PlayPauseCommand
        {
            get => new MvvmHelpers.Commands.Command(() =>
            {
                Audio.PlayPause();
                ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg;
                OnPropertyChanged(nameof(ResourceClass));
            });
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tracks = await App.Connections.BaseDataBaseService.DataBase.Table<Track>().ToListAsync();
            var searchres = tracks.Where((c) => c.Title.StartsWith(e.NewTextValue, StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (searchres != null && searchres.Count != 0)
            {
                Tracks = new ObservableCollection<Track>(searchres);
            }
            else
            {
                Tracks.Clear();
            }
        }
    }
}
