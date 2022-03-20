using MusicHUB.Helpers;
using MusicHUB.Models;
using MusicHUB.ViewModels.Properties;
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
        private double startpositionLineX;
        private double endpositionLineX;


        #region Propreties     
        public PlayerPosition playerPosition { get; set; }
        public double SliderKoefficient { get; set; }
        public double StartpositionLineX { get => startpositionLineX; set { startpositionLineX = value; OnPropertyChanged(nameof(StartpositionLineX)); } }
        public double EndpositionLineX { get => endpositionLineX; set { endpositionLineX = value; OnPropertyChanged(nameof(EndpositionLineX)); } }
        public int StarttrimMax { get; set; }
        public int EndtrimMax { get; set; }
        public string FileOutputPath { get => fileOutputPath; set { fileOutputPath = value; OnPropertyChanged(nameof(FileOutputPath)); } }
        public int StartTrimPosition
        {
            get => startTrimPosition;
            set 
            {
                startTrimPosition = value;
                if(SliderKoefficient != 0) StartpositionLineX = (startTrimPosition * SliderKoefficient);
                playerPosition.CurrentPosition = startTrimPosition;
                OnPropertyChanged(nameof(playerPosition));

                if (startTrimPosition + 5000 >= endTrimPosition && EndTrimPosition < EndtrimMax)
                {
                    EndTrimPosition = startTrimPosition + 5000;
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
                if (SliderKoefficient != 0) EndpositionLineX = (endTrimPosition * SliderKoefficient);
                playerPosition.Duration = endTrimPosition;
                OnPropertyChanged(nameof(playerPosition));

                    if (startTrimPosition > endTrimPosition - 5000 && StartTrimPosition > 0)
                {
                    StartTrimPosition = endTrimPosition - 5000;
                }
                OnPropertyChanged(nameof(EndTrimPosition)); 
            } 
        }
#endregion Properties

        public CutPopupPageViewModel(Track track)
        {
            playerPosition = new PlayerPosition();
            musicFileTrimmer = new MusicFileTrimmer();
            fileInputPath = track.Uri;
            StartTrimPosition = 0;
            EndTrimPosition = track.Duration;
            StarttrimMax = track.Duration;
            EndtrimMax = track.Duration;
            this.track = track;
            SliderKoefficient = 340F / (double)EndtrimMax;
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
