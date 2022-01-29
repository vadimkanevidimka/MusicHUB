using Android.Content;
using Android.Media;
using MusicHUB;
using MusicHUB.Droid;
using MusicHUB.Models;
using SimpleAudioForms.Droid;
using System;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Track> Queue = new MusicFilesCollector().GetTracks(new string[] { "/storage/emulated/0/Music/", "/storage/emulated/0/Download/" });
        private int Position { get; set; }
        private Track CurrentTrack { get; set; }

        bool IAudio.IsExists { get { return !(player is null); } }
        int IAudio.Time { get => player != null ? player.CurrentPosition : 0; }
        bool IAudio.Looping { get => player.Looping; }
        int IAudio.GetVolumeLevel => AudioManager.GetStreamVolume(Android.Media.Stream.Music);
        int IAudio.GetMaxVolumeLevel => AudioManager.GetStreamMaxVolume(Android.Media.Stream.Music);

        public ObservableCollection<Track> Tracks => Queue;

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
            if (OnCompleted != null) OnCompleted(sender, e);
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
            if (CurrentTrack is null) CurrentTrack = track;
            else if (CurrentTrack.Id == track.Id) return;
            Position = track.Id;
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
            if (Position != Queue.Count - 1)
            {
                PlayAudioFile(Queue[Position + 1]);
            }
        }

        public void Prev()
        {
            if (Position != 0)
            {
                PlayAudioFile(Queue[Position - 1]);
            }
        }

        public Track GetCurrentTrack()
        {
            return Queue[Position];
        }

        public void PlayPause()
        {
            if (player.IsPlaying) player.Pause();
            else player.Start();
        }

        public void Shuffle()
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
    }
}
