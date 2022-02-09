using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.ViewModels
{
    class QueuePopupPageViewModel : INotifyPropertyChanged
    {
        public QueuePopupPageViewModel()
        {
            queue = DependencyService.Get<IAudio>().Tracks;
        }

        private IList<Track> queue;

        public IList<Track> Queue
        {
            get { return queue; }
            set
            {
                queue = value;
                OnPropertyChanged("Title");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
