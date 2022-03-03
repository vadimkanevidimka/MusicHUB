using Android.Content;
using MusicHUB.EventArgumentss;
using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MusicHUB
{
    public interface IAudio
    {
        int Time { get; }
        Context GetContext { get; }
        bool IsExists { get; }
        bool IsLooping { get; }
        bool IsPlaying { get; }
        bool Looping { get;}
        int GetVolumeLevel { get; }
        int GetMaxVolumeLevel { get; }
        ObservableCollection<Track> Tracks { get; }

        event EventHandler OnCompleted;
        event EventHandler OntrackChanged;
        event PlayerTimeEventHandler OnPlayerTimeChanged;
        Track GetCurrentTrack();
        object[] GetInfo();
        void Next();
        void Prev();
        void Pause();
        Task Shuffle();
        void Countinue();
        void PlayPause();
        void LoopChange();
        void PlayerDispose();
        void SetTime(double Time);
        void AddToQueue(Track track);
        void PlayAudioFile(Track track);
        void SetVolumeLevel(int volume);
        void RemoveFromQueue(Track track);
        void SetQueue(List<Track> Tracks);
    }
}
