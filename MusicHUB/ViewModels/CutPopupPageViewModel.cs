using MusicHUB.Helpers;
using MusicHUB.Models;
using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MusicHUB.ViewModels
{
    class CutPopupPageViewModel : BindableObject
    {
        private int startTrimPosition;
        private int endTrimPosition;
        private MusicFileTrimmer musicFileTrimmer;
        private string fileInputPath;
        private string fileOutputPath;
        private Track track;

        public int StarttrimMax { get; set; }
        public int EndtrimMax { get; set; }
        public string FileOutputPath { get => fileOutputPath; set { fileOutputPath = value; OnPropertyChanged(nameof(FileOutputPath)); } }
        public int StartTrimPosition
        {
            get => startTrimPosition;
            set 
            {
                startTrimPosition = value;

                if(startTrimPosition + 50000 >= endTrimPosition && EndTrimPosition < EndtrimMax)
                {
                    EndTrimPosition = startTrimPosition + 50000;
                }
                OnPropertyChanged(nameof(StartTrimPosition)); 
            } 
        }
        public int EndTrimPosition 
        {
            get => endTrimPosition;
            set 
            {
                endTrimPosition = value;

                if (startTrimPosition > endTrimPosition - 50000 && StartTrimPosition > 0)
                {
                    StartTrimPosition = endTrimPosition - 50000;
                }
                OnPropertyChanged(nameof(EndTrimPosition)); 
            } 
        }

        public CutPopupPageViewModel(Track track)
        {
            musicFileTrimmer = new MusicFileTrimmer();
            fileInputPath = track.Uri;
            StartTrimPosition = 0;
            EndTrimPosition = track.Duration;
            StarttrimMax = track.Duration;
            EndtrimMax = track.Duration;
            this.track = track;
        }

        public ICommand CutCommand
        {
            get => new Xamarin.Forms.Command(() =>
            {
                musicFileTrimmer.TrimWavFile(fileInputPath, $"/storage/emulated/0/{Android.OS.Environment.DirectoryMusic}/{fileOutputPath}.mp3", new TimeSpan(startTrimPosition), new TimeSpan(track.Duration - endTrimPosition), track);
                App.Current.MainPage.Navigation.PopPopupAsync();
            });
        }

        public ICommand SetAsRingtoneCommand
        {
            get => new Xamarin.Forms.Command(() =>
            {
                musicFileTrimmer.SetAsRingtone(fileInputPath, fileOutputPath, new TimeSpan(startTrimPosition), new TimeSpan(endTrimPosition), track);
                App.Current.MainPage.Navigation.PopPopupAsync();
            });
        }
    }
}
