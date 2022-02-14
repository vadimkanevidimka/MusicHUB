using MusicHUB.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;
using Genius.Models.Response;
using MusicHUB.DependencyInjection;
using MusicHUB.Pages;
using Rg.Plugins.Popup.Extensions;
using Genius.Models.Artist;
using Genius.Models;
using System.Linq;
using MusicHUB.ViewModels.Properties;
using MvvmHelpers.Commands;
using Google.Android.Material.BottomSheet;
using Android.Widget;
using System.ComponentModel;

namespace MusicHUB.ViewModels
{
    public class PlayerViewModel : BindableObject
    {
        private readonly IAudio Audio = DependencyService.Get<IAudio>();

        private Toast ToastMessage { get; set; }

        private Timer timer;

        private Connections Connections { get; set; }

        private INavigation Navigation { get; set; }

        public PlayerPosition PlayerPosition { get; set; }

        public int VolumeLevel { get; set; }

        public int MaxVolumeLevel { get; set; }

        public NotifyTaskCompletion<Color> PrimaryColor { get; set; } 

        public ResourceClassPlayerPage ResourceClass { get; set; }

        public NotifyTaskCompletion<IList<Artist>> Artists { get; set; }

        public ObservableCollection<Track> Tracks { get => Audio.Tracks; }

        public PlayerViewModel(INavigation navigation, Connections connections)
        {
            Navigation = navigation;
            Connections = connections;
            PlayerPosition = new PlayerPosition();
            MaxVolumeLevel = Audio.GetMaxVolumeLevel;
            VolumeLevel = Audio.GetVolumeLevel;
            ResourceClass = new ResourceClassPlayerPage();
            Audio.OnCompleted += (obj, e) => OnPropertyChanged(nameof(CurrentTrack));
            OnPropertyChanged(nameof(CurrentTrack));
            InitTimer();
        }

        public Artist CurrentArtist { get; set; }

        public Color BackGround { get => BackGround; 
            set 
            {
                BackGround = value;
            }
        }

        public ICommand SetVolumeCommand { 
            get => new MvvmHelpers.Commands.Command(() => Audio.SetVolumeLevel(VolumeLevel));
        }

        public Track CurrentTrack
        {
            get
            {
                return Audio.GetCurrentTrack();
            }
        }

        public async void TrackChanging()
        {
            OnPropertyChanged(nameof(CurrentTrack));
            await IsTrackLiked(CurrentTrack);
            Artists = new NotifyTaskCompletion<IList<Artist>>(GetArtists());
            OnPropertyChanged(nameof(Artists));
            InitTimer();
            await Task.Run(() => { Task.Delay(1000); ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg; } );
        }

        public ICommand SeekPlayerSlider
        {
            get => new MvvmHelpers.Commands.Command(() =>
            {
                Audio.SetTime(PlayerPosition.CurrentPosition);
                InitTimer();
            });
        }

        public ICommand DragPlayerStartedCommand
        {
            get => new MvvmHelpers.Commands.Command(()=>
                {
                    timer.Dispose();
                });
        }

        public ICommand TrackChange
        {
            get => new MvvmHelpers.Commands.Command(c =>
            {
                timer.Dispose();
                Audio.PlayAudioFile((Track)c);
                TrackChanging();
                ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg;
            });
        }

        public ICommand NextTrack {
            get => new MvvmHelpers.Commands.Command(() =>
            {
                timer.Dispose();
                Audio.Next();
                TrackChanging();
                ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg;
            });
        }

        public ICommand PrevTrack {
            get => new MvvmHelpers.Commands.Command(() =>
            {
                timer.Dispose();
                Audio.Prev();
                TrackChanging();
            }); 
        }

        public ICommand PlayPause {
            get => new MvvmHelpers.Commands.Command(() =>
            {
                Audio.PlayPause();
                ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg;
                OnPropertyChanged(nameof(ResourceClass));
            });
        }

        public AsyncCommand ShuffleCommand {
            get => new AsyncCommand(async () => await Shufle());
        }

        private async Task Shufle()
        {
            Audio.Shuffle();
            ResourceClass.Shufle = ResourceClassPlayerPage.ShuffledImg;
            OnPropertyChanged(nameof(Tracks));
            OnPropertyChanged(nameof(ResourceClass));
        }

        public ICommand ArtistPageCommand
        {
            get => new MvvmHelpers.Commands.Command(
            async (c) =>
            {
                await App.Current.MainPage.Navigation.PopModalAsync();
                await App.Current.MainPage.Navigation.PushAsync(new ArtistPage(CurrentArtist, Connections));
            });
        }

        public ICommand RepeatCommand {
            get => new MvvmHelpers.Commands.Command(() =>
            {
                Audio.LoopChange();
                ResourceClass.Repeat = Audio.IsLooping ? ResourceClassPlayerPage.LoopedImg : ResourceClassPlayerPage.LoopImg;
                OnPropertyChanged(nameof(ResourceClass));
            });
        }

        public ICommand OpenOptions
        {
            get => new MvvmHelpers.Commands.AsyncCommand(async () => { await this.Navigation.PushPopupAsync(new PopUpContextActionsOnTrack(CurrentTrack, ContextActions.TrackContextActionState.AtPlayerPage), true); OnPropertyChanging(nameof(Tracks)); });
        }

        public ICommand LikeCommand
        {
            get => new MvvmHelpers.Commands.AsyncCommand(async () =>
            {
                if (await IsTrackLiked(CurrentTrack))
                {
                    await Connections.BaseDataBaseService.DataBase.QueryAsync<DBLikedTracks>("Delete from LikedTracks where TrackId = ?", CurrentTrack.Id);
                    Toast.MakeText(Audio.GetContext, $"{CurrentTrack.Title} удалена из любимого", ToastLength.Short).Show();
                }
                else
                {
                    await Connections.BaseDataBaseService.DataBase.InsertAsync(new DBLikedTracks() { TrackId = CurrentTrack.Id });
                    Toast.MakeText(Audio.GetContext, $"{CurrentTrack.Title} добавлена в любимое", ToastLength.Short).Show();
                }
                await IsTrackLiked(CurrentTrack);
            });
        }

        private async Task<bool> IsTrackLiked(Track track)
        {
            var liked = await Connections.BaseDataBaseService.DataBase.Table<DBLikedTracks>().ToListAsync();
            if(liked.Exists((context) => context.TrackId == CurrentTrack.Id))
            {
                ResourceClass.Like = ResourceClassPlayerPage.LikedImg;
                OnPropertyChanged(nameof(ResourceClass));
                return true;
            }
            else
            {
                ResourceClass.Like = ResourceClassPlayerPage.LikeImg;
                OnPropertyChanged(nameof(ResourceClass));
                return false;
            }
        }

        public ICommand ClosePlayerCommand => new MvvmHelpers.Commands.AsyncCommand( async () => {await this.Navigation.PopModalAsync(); Audio.OnCompleted -= (obj, e) => OnPropertyChanged(nameof(CurrentTrack)); });

        private async Task<IList<Artist>> GetArtists()
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

        private void InitTimer()
        {
            TimerCallback callback = CurrentTime;
            timer = new Timer(callback, null, 0, 500);
        }

        private void CurrentTime(object c)
        {
            PlayerPosition.CurrentPosition = Audio.Time;
            VolumeLevel = Audio.GetVolumeLevel;
            OnPropertyChanged(nameof(PlayerPosition));
            OnPropertyChanged(nameof(VolumeLevel));
        }
    }
}
