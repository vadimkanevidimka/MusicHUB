using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.ViewModels.Properties
{
    public class PlayerPosition
    {
        public const int Minimum = 0;

        public int CurrentPosition { get; set; }

        public int Maximum { get => DependencyService.Get<IAudio>().GetCurrentTrack().Duration; }

        public string CurrentPOsitionString
        {
            get
            {
                return GetStringTime(CurrentPosition);
            }
        }

        public string DurationString
        {
            get
            {
                return GetStringTime(Maximum);
            }
        }

        private string GetStringTime(int timemilisec)
        {
            int min = (timemilisec / 1000) / 60;
            int sec = (timemilisec / 1000) % 60;
            return sec < 10 ? $"{min}:0{sec}" : $"{min}:{sec}";
        }
    }
}
