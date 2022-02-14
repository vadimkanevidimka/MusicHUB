using Android.Content;
using Android.Media;
using MusicHUB;
using MusicHUB.DataBaseServices;
using MusicHUB.Droid;
using MusicHUB.Models;
using SimpleAudioForms.Droid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<Track> Queue = new List<Track>();
        private int Position { get; set; }
        private Track CurrentTrack { get; set; }

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
        }

        private EventHandler OnCompleted;
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
            player?.Reset();
            player = new MediaPlayer();
            player.SetAudioStreamType(Stream.Music);
            player.Prepared += (s, e) =>
            {
                player.Start();
            };
            player.Completion += setNext;
        }

        public void SetVolumeLevel(int volume)
        {
            AudioManager.SetStreamVolume(Stream.Music, volume, VolumeNotificationFlags.AllowRingerModes);
        }

        private void setNext(object sender, EventArgs e)
        {
            Next();
        }

        public void LoopChange()
        {
            player.Looping = !player.Looping;
        }

        void IAudio.PlayerDispose()
        {
            player.Dispose();
        }

        void IAudio.Pause() => player.Pause();

        void IAudio.Countinue() => player.Start();

        public void PlayAudioFile(Track track)
        {
            if (track is null) return;
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
        }

        object[] IAudio.GetInfo() => player.GetTrackInfo();

        void IAudio.SetTime(double Time) => player.SeekTo((int)Time);

        public void Next()
        {
            if (Queue is null) return;
            if (Position != Queue.Count - 1)
            {
                PlayAudioFile(Queue[Position + 1]);
            }
            if (OnCompleted != null) OnCompleted(null, new EventArgs());
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
            Queue.Remove(track);
        }
    }
}
