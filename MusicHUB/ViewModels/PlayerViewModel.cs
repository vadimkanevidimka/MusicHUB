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
using Genius.Models.Artist;
using Genius.Models;
using System.Linq;

namespace MusicHUB.ViewModels
{
    public class PlayerViewModel : BindableObject
    {
        private readonly IAudio Audio = DependencyService.Get<IAudio>();

        private Timer timer;

        private Connections Connections { get; set; }

        private INavigation Navigation { get; set; }

        public int VolumeLevel { get; set; }

        public int MaxVolumeLevel { get; set; }

        public NotifyTaskCompletion<Color> PrimaryColor { get => new NotifyTaskCompletion<Color>(DependencyService.Get<IImage>().ConfigColorAsync(CurrentTrack.Uri)); } 

        public ResourceClassPlayerPage ResourceClass { get; set; }

        public NotifyTaskCompletion<IEnumerable<Artist>> Artists { get; set; }

        public ObservableCollection<Track> Tracks { get => Audio.Tracks; }

        public PlayerViewModel(INavigation navigation, Connections connections)
        {
            Navigation = navigation;
            Connections = connections;
            MaxVolumeLevel = Audio.GetMaxVolumeLevel;
            VolumeLevel = Audio.GetVolumeLevel;
            ResourceClass = new ResourceClassPlayerPage();
            Audio.OnCompleted += (obj, e) => OnPropertyChanged(nameof(CurrentTrack));
            OnPropertyChanged(nameof(CurrentTrack));
            InintTimer();
        }

        public Artist CurrentArtist { get; set; }

        public Color BackGround { get => BackGround; 
            set 
            {
                BackGround = value;
            }
        }

        public string PlayedTimeString 
        { 
            get 
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
                TrackChanging();
                Artists = new NotifyTaskCompletion<IEnumerable<Artist>>(GetArtists());
                InintTimer();
                ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg;
            });
        }

        public ICommand NextTrack {
            get => new Command(() =>
            {
                timer.Dispose();
                Audio.Next();
                TrackChanging();
                Artists = new NotifyTaskCompletion<IEnumerable<Artist>>(GetArtists());
                InintTimer();
                ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg;
            });
        }

        public ICommand PrevTrack {
            get => new Command(() =>
            {
                timer.Dispose();
                Audio.Prev();
                TrackChanging();
                Artists = new NotifyTaskCompletion<IEnumerable<Artist>>(GetArtists());
                InintTimer();
                ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg;
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
            get => new Command(async () =>
            {
                Audio.Shuffle();
                ResourceClass.Shufle = ResourceClassPlayerPage.ShuffledImg;
                OnPropertyChanged(nameof(ResourceClass));
            });
        }

        public Command ArtistPageCommand
        {
            get => new Command((c) => this.Navigation.PushModalAsync(new ArtistPage(CurrentArtist, Connections), false));
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

        private async Task<IEnumerable<Artist>> GetArtists()
        {
            List<Artist> ArtistAndImage = new List<Artist>();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var rezult = await Connections.GeniusClient.SearchClient.Search($"{CurrentTrack.Title} {CurrentTrack.Artist}");
                var artists = await Connections.GeniusClient.SongClient.GetSong(rezult.Response.Hits.First().Result.Id);
                
                ArtistAndImage.Add(artists.Response.Song.PrimaryArtist);
                if (artists.Response.Song.FeaturedArtists != null)
                {
                    ArtistAndImage.AddRange(artists.Response.Song.FeaturedArtists);
                }
                return ArtistAndImage;
            }
            return ArtistAndImage;
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
