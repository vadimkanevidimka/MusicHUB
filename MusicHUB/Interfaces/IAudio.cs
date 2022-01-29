using Android.Content;
using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        Track GetCurrentTrack();
        object[] GetInfo();
        void Next();
        void Prev();
        void Pause();
        void Shuffle();
        void Countinue();
        void PlayPause();
        void LoopChange();
        void PlayerDispose();
        void SetVolumeLevel(int volume);
        void SetTime(double Time);
        void PlayAudioFile(Track track);
    }
}
