using MusicHUB.Interfaces;
using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Genius;
using System.Threading.Tasks;
using Genius.Models.Response;
using MusicHUB.DependencyInjection;
using MusicHUB.Pages;
using Rg.Plugins.Popup.Extensions;

namespace MusicHUB.ViewModels
{
    public class PlayerViewModel : BindableObject
    {
        private readonly IAudio Audio = DependencyService.Get<IAudio>();

        private Timer timer;

        private Connections Connections { get; set; }

        private Action TrackChanged { get; set; }

        private INavigation Navigation { get; set; }

        public int VolumeLevel { get; set; }

        public int MaxVolumeLevel { get; set; }

        public NotifyTaskCompletion<Color> PrimaryColor { get => new NotifyTaskCompletion<Color>(DependencyService.Get<IImage>().ConfigColorAsync(CurrentTrack.Uri)); } 

        public ResourceClassPlayerPage ResourceClass { get; set; }

        public NotifyTaskCompletion<IEnumerable<SearchResponse>> Artists { get; set; }

        public ObservableCollection<Track> Tracks { get => Audio.Tracks; }

        public PlayerViewModel(INavigation navigation, Connections connections)
        {
            Navigation = navigation;
            TrackChanged += TrackChanging;
            Connections = connections;
            MaxVolumeLevel = Audio.GetMaxVolumeLevel;
            VolumeLevel = Audio.GetVolumeLevel;
            ResourceClass = new ResourceClassPlayerPage();
            Audio.OnCompleted += (obj, e) => OnPropertyChanged(nameof(CurrentTrack));
            OnPropertyChanged(nameof(CurrentTrack));
            InintTimer();
        }

        public Color BackGround { get => BackGround; 
            set 
            {
                BackGround = value;
            }
        }

        public string PlayedTimeString { get 
            {
                OnPropertyChanged(nameof(PlayedTime));
                int min = (PlayedTime / 1000) / 60;
                int sec = (PlayedTime / 1000) % 60;
                return sec < 10 ? $"{min}:0{sec}" : $"{min}:{sec}";
            }
        }

        public int PlayedTime { get; set; }

        public ICommand SetVolumeCommand { 
            get => new Command(() => Audio.SetVolumeLevel(VolumeLevel));
        }

        public Track CurrentTrack
        {
            get
            {
                return Audio.GetCurrentTrack();
            }
        }

        public void TrackChanging()
        {
            OnPropertyChanged(nameof(CurrentTrack));
        }

        public ICommand TrackChange
        {
            get => new Command(c =>
            {
                timer.Dispose();
                Audio.PlayAudioFile((Track)c);
                Artists = new NotifyTaskCompletion<IEnumerable<SearchResponse>>(GetArtists());
                InintTimer();
            });
        }

        public ICommand NextTrack {
            get => new Command(() =>
            {
                timer.Dispose();
                Audio.Next();
                TrackChanged();
                InintTimer();
            });
        }

        public ICommand PrevTrack {
            get => new Command(() =>
            {
                timer.Dispose();
                Audio.Prev();
                TrackChanged();
                InintTimer();
            }); 
        }

        public ICommand PlayPause {
            get => new Command(() =>
            {
                Audio.PlayPause();
                ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg;
                OnPropertyChanged(nameof(ResourceClass));
            });
        }

        public Command ShuffleCommand {
            get => new Command(() =>
            {
                Audio.Shuffle();
                ResourceClass.Shufle = ResourceClassPlayerPage.ShuffledImg;
                OnPropertyChanged(nameof(ResourceClass));
            });
        }

        public Command ArtistPageCommand
        {
            get => new Command((c) => this.Navigation.PushModalAsync(new ArtistPage((SearchResponse)c, Connections)));
        }

        public Command RepeatCommand {
            get => new Command(() =>
            {
                Audio.LoopChange();
                ResourceClass.Repeat = Audio.IsLooping ? ResourceClassPlayerPage.LoopedImg : ResourceClassPlayerPage.LoopImg;
                OnPropertyChanged(nameof(ResourceClass));
            });
        }

        public ICommand OpenOptions
        {
            get => new Command(() => this.Navigation.PushPopupAsync(new PopUpPageFromBottom(CurrentTrack)));
        }

        private async Task<IEnumerable<SearchResponse>> GetArtists()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                List<SearchResponse> ArtistAndImage = new List<SearchResponse>();
                var artists = CurrentTrack.Artist.Split(new char[] { ',' });
                foreach (var item in artists)
                {
                    var rezult = await Connections.GeniusClient.SearchClient.Search(item);
                    ArtistAndImage.Add(rezult);
                }
                return ArtistAndImage;
            }
            return new List<SearchResponse>();
        }

        private void InintTimer()
        {
            TimerCallback callback = CurrentTime;
            timer = new Timer(callback, null, 0, 500);
        }

        private NetworkAccess Ethernet()
        {
            return Connectivity.NetworkAccess;
        }

        private void CurrentTime(object c)
        {
            PlayedTime = Audio.Time;
            OnPropertyChanged(nameof(PlayedTimeString));
        }
    }
}
