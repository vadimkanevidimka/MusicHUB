using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.Media.Session;
using AndroidX.Core.App;
using MediaManager;
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
        private IMediaManager MediaManager = CrossMediaManager.Current;
        private List<Track> Queue = new List<Track>();
        private int Position { get; set; }
        private Track CurrentTrack { get; set; }
        private Timer timer;

        bool IAudio.IsExists { get { return !(MediaManager is null); } }
        int IAudio.Time { get => MediaManager != null ? MediaManager.Position.Milliseconds : 0; }
        bool IAudio.Looping { get => (int)MediaManager.RepeatMode == 1; }
        int IAudio.GetVolumeLevel => AudioManager.GetStreamVolume(Stream.Music);
        int IAudio.GetMaxVolumeLevel => AudioManager.GetStreamMaxVolume(Stream.Music);

        public ObservableCollection<Track> Tracks => new ObservableCollection<Track>(Queue);

        public bool IsPlaying => MediaManager.IsPlaying();

        public bool IsLooping => (int)MediaManager.RepeatMode == 1;

        public Context GetContext => Android.App.Application.Context;

        public AudioService()
        {
            InitTimer();
            MediaManager.MediaItemChanged += (s, e) => { if (OntrackChanged != null) OntrackChanged(this, new EventArgs()); };
            MediaManager.MediaItemFinished += (s, e) => { Next(); if (OnCompleted != null) OnCompleted(this, new EventArgs()); };
            MediaManager.StateChanged += (s, e) => { if (OnstateChanged != null) OnstateChanged(this, new EventArgs()); };
        }

        private EventHandler OnCompleted;
        private EventHandler OntrackChanged;
        private PlayerTimeEventHandler OnPlayerTimeChanged;
        private EventHandler OnstateChanged;

        event EventHandler IAudio.OnstateChanged
        {
            add
            {
                OnstateChanged += value;
            }

            remove
            {
                OnstateChanged -= value;
            }
        }

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

        private void InitTimer()
        {
            TimerCallback callback = (c) => { if (OnPlayerTimeChanged != null) OnPlayerTimeChanged(new PlayerTimeEventArgs((int)MediaManager.Position.TotalMilliseconds, (int)MediaManager.Duration.TotalMilliseconds)); };
            timer = new Timer(callback, null, 0, 500);
        }

        public void SetVolumeLevel(int volume)
        {
            AudioManager.SetStreamVolume(Stream.Music, volume, VolumeNotificationFlags.ShowUi);
        }

        public void LoopChange()
        {
            if ((int)MediaManager.RepeatMode == 1) MediaManager.RepeatMode = 0;
            else MediaManager.RepeatMode = (MediaManager.Playback.RepeatMode)1;
        }

        void IAudio.PlayerDispose()
        {
            MediaManager.Dispose();
        }

        void IAudio.Pause() => MediaManager.Pause();

        void IAudio.Countinue() => MediaManager.Play();

        public async Task PlayAudioFile(Track track)
        {
            if (track is null) return;
            if (CurrentTrack != null && CurrentTrack != track) AddToRecentlyPlayed(CurrentTrack);
            if (CurrentTrack is null) CurrentTrack = track;
            else if (CurrentTrack.Id == track.Id) return;
            Position = Queue.IndexOf(track);
            CurrentTrack = track;
            await MediaManager.Play(track.Uri);
            OntrackChanged(this, new EventArgs());
        }

        async void IAudio.SetTime(double Time)
        {
            await MediaManager.SeekTo(new TimeSpan((long)Time * 10000));
        }

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
            if (MediaManager.IsPlaying()) MediaManager.Pause();
            else MediaManager.Play();
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
            if (Queue.Count == 0)
            {
                Queue.Add(track);
                PlayAudioFile(track);
                return;
            }
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

        public void AddToQueue(IEnumerable<Track> tracks)
        {
            if (Queue.Count == 0)
            {
                Queue.AddRange(tracks);
                PlayAudioFile(Queue[0]);
                return;
            }
            Queue.InsertRange(Position + 1, tracks);
        }
    }
}
