using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.Media.Session;
using AndroidX.Core.App;
using MusicHUB;
using MusicHUB.DataBaseServices;
using MusicHUB.Droid;
using MusicHUB.EventArgumentss;
using MusicHUB.Interfaces;
using MusicHUB.Models;
using SimpleAudioForms.Droid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: Dependency(typeof(AudioService))]
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SimpleAudioForms.Droid
{
    public class AudioService : IAudio
    {
        private AudioManager AudioManager = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
        private MediaPlayer player;
        MediaSessionCompat MediaSessionCompat { get; set; }
        private NotificationManager notificationManager = (NotificationManager)DependencyService.Get<IAndroidSystem>().AppContext.GetSystemService(Context.NotificationService);
        private List<Track> Queue = new List<Track>();
        private int Position { get; set; }
        private Track CurrentTrack { get; set; }
        private Timer timer;

        bool IAudio.IsExists { get { return !(player is null); } }
        int IAudio.Time { get => player != null ? player.CurrentPosition : 0; }
        bool IAudio.Looping { get => player.Looping; }
        int IAudio.GetVolumeLevel => AudioManager.GetStreamVolume(Stream.Music);
        int IAudio.GetMaxVolumeLevel => AudioManager.GetStreamMaxVolume(Stream.Music);

        public ObservableCollection<Track> Tracks => new ObservableCollection<Track>(Queue);

        public bool IsPlaying => player.IsPlaying;

        public bool IsLooping => player.Looping;

        public Context GetContext => Android.App.Application.Context;

        public AudioService()
        {
            Init();
            InitTimer();
            MediaSessionCompat = new MediaSessionCompat(DependencyService.Get<IAndroidSystem>().AppContext, "MusicHUB");
            MediaSessionCompat.Active = true;
        }

        private EventHandler OnCompleted;
        private EventHandler OntrackChanged;
        private PlayerTimeEventHandler OnPlayerTimeChanged;

        event PlayerTimeEventHandler IAudio.OnPlayerTimeChanged
        {
            add
            {
                OnPlayerTimeChanged += value;
            }

            remove
            {
                OnPlayerTimeChanged -= value;
            }
        }

        event EventHandler IAudio.OntrackChanged
        {
            add
            {
                OntrackChanged += value;
            }

            remove
            {
                OntrackChanged -= value;
            }
        }

        event EventHandler IAudio.OnCompleted
        {
            add
            {
                OnCompleted += value;
            }

            remove
            {
                OnCompleted -= value;
            }
        }

        private void Init()
        {
            player?.Stop();
            player = null;
            player = new MediaPlayer();
            player.SetAudioStreamType(Stream.Music);
            player.SetWakeMode(DependencyService.Get<IAndroidSystem>().AppContext, Android.OS.WakeLockFlags.Partial);
            player.Prepared += (s, e) =>
            {
                player.Start();
            };
            player.Completion += (sender, args) => Next();
        }

        private void InitTimer()
        {
            TimerCallback callback = (c) => { if (OnPlayerTimeChanged != null && player != null) OnPlayerTimeChanged(new PlayerTimeEventArgs(player.CurrentPosition, player.Duration)); };
            timer = new Timer(callback, null, 0, 1000);
        }

        public void SetVolumeLevel(int volume)
        {
            AudioManager.SetStreamVolume(Stream.Music, volume, VolumeNotificationFlags.ShowUi);
        }

        public void LoopChange()
        {
            player.Looping = !player.Looping;
        }

        void IAudio.PlayerDispose()
        {
            player = null;
        }

        void IAudio.Pause() => player.Pause();

        void IAudio.Countinue() => player.Start();

        public void PlayAudioFile(Track track)
        {
            if (track is null) return;
            if (CurrentTrack != null) AddToRecentlyPlayed(CurrentTrack);
            if (CurrentTrack is null) CurrentTrack = track;
            else if (CurrentTrack.Id == track.Id) return;
            Position = Queue.IndexOf(track);
            CurrentTrack = track;
            if (player != null)
            {
                Init();
                player.SetDataSource(track.Uri);
                player.Prepare();
            }
            Notificate();
            OntrackChanged(this, new EventArgs());
        }

        object[] IAudio.GetInfo() => player.GetTrackInfo();

        void IAudio.SetTime(double Time) => player.SeekTo((int)Time);

        public void Next()
        {
            if (Queue is null || Queue.Count == 0) return;
            if (Position != Queue.Count - 1)
            {
                PlayAudioFile(Queue[Position + 1]);
            }
            if (OnCompleted != null) OnCompleted(this, new EventArgs());
        }

        public void Prev()
        {
            if (Queue is null) return;
            if (Position != 0)
            {
                PlayAudioFile(Queue[Position - 1]);
            }
        }

        public Track GetCurrentTrack()
        {
            if (Queue == null || Queue.Count == 0)
            {
                return null;
            }
            return Queue[Position];
        }

        public void PlayPause()
        {
            if (player.IsPlaying) player.Pause();
            else player.Start();
        }

        public async Task Shuffle()
        {
           await Task.Run(() => ShuffleQueue());
        }

        private void ShuffleQueue()
        {
            Random rand = new Random();

            for (int i = Queue.Count - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                Track tmp = Queue[j];
                Queue[j] = Queue[i];
                Queue[i] = tmp;
            }
        }

        public void SetQueue(List<Track> Tracks)
        {
            Queue = Tracks;
        }

        public void AddToQueue(Track track)
        {
            Queue.Insert(Position + 1, track);
        }

        public void RemoveFromQueue(Track track)
        {
            if (track is null)
            {
                return;
            }

            Queue?.Remove(track);
        }

        private async void AddToRecentlyPlayed(Track playedTrack)
        {
            await App.Connections.BaseDataBaseService.DataBase.InsertAsync(new RecentlyPlayed() { TrackId = playedTrack.Id, ListenTime = DateTime.Now });
        }

        private void Notificate()
        {
            Notification notification = new NotificationCompat.Builder(DependencyService.Get<IAndroidSystem>().AppContext)
                .SetSmallIcon(Resource.Drawable.play)
                .SetContentTitle(CurrentTrack.Title)
                .SetContentText(CurrentTrack.Artist)
                .SetColorized(true)
                .SetAllowSystemGeneratedContextualActions(true)
                .SetLargeIcon(DependencyService.Get<IImage>().GetBitmap(CurrentTrack.Uri))
                .SetStyle(new AndroidX.Media.App.NotificationCompat.DecoratedMediaCustomViewStyle())
                .SetPriority((int)NotificationPriority.Max)
                .SetAutoCancel(false)
                .SetVisibility((int)NotificationVisibility.Public)
                .SetChannelId("108")
                .Build();
            NotificationChannel notificationChannel = new NotificationChannel("108", CurrentTrack.Title, NotificationImportance.High);
            notificationManager.CreateNotificationChannel(notificationChannel);
            notificationManager.Notify(0, notification);
        }
    }
}
