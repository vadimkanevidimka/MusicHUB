using Android.Widget;
using MusicHUB.Interfaces;
using MusicHUB.Models;
using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MusicHUB.ViewModels
{
    class QueuePopupPageViewModel : BindableObject
    {
        public ObservableCollection<Track> Queue { get => queue; set { queue = value; OnPropertyChanged(nameof(Queue)); } }
        private ObservableCollection<Track> queue;
        public string Header { get => header; set { header = value; OnPropertyChanged(nameof(Header)); } }
        private string header;
        public string SwipeText { get => swipetext; set { swipetext = value; OnPropertyChanged(nameof(SwipeText)); } }
        private string swipetext;
        public Color SwipeColor { get => swipeColor; set { swipeColor = value; OnPropertyChanged(nameof(SwipeColor)); } }
        private Color swipeColor;

        private Album album;
        public ICommand AddToAlbumCommand 
        { 
            get => new AsyncCommand<Track>(async (track) =>
            {
                try
                {
                    await App.Connections.BaseDataBaseService.DataBase.InsertAsync(new AlbumsTracks() { AlbumId = album.Id, TrackId = track.Id });
                    await App.Current.MainPage.Navigation.PopPopupAsync();
                    Toast.MakeText(DependencyService.Get<IAndroidSystem>().AppContext, $"{track.Title} добавлен в альбом {album.Title}", ToastLength.Short).Show();
                }
                catch (Exception)
                {
                    Toast.MakeText(DependencyService.Get<IAndroidSystem>().AppContext, $"{track.Title} не добавлен в альбом {album.Title}", ToastLength.Short).Show();
                }
            });
        }

        public ICommand RemoveFromQueueCommand
        {
            get => new AsyncCommand<Track>(async (track) =>
            {
                try
                {
                    Queue.Remove(track);
                    DependencyService.Get<IAudio>().RemoveFromQueue(track);
                    Toast.MakeText(DependencyService.Get<IAndroidSystem>().AppContext, $"{track.Title} удален из очереди", ToastLength.Short).Show();
                }
                catch (Exception)
                {
                    Toast.MakeText(DependencyService.Get<IAndroidSystem>().AppContext, $"{track.Title} не удален из очереди", ToastLength.Short).Show();
                }
            });
        }

        private bool isvisibleaddtoalbum;
        public bool IsVisibleAddToAlbum { get => isvisibleaddtoalbum; set { isvisibleaddtoalbum = value; OnPropertyChanged(nameof(IsVisibleAddToAlbum)); } }

        private bool isvisibleremovefromqueue;
        public bool IsVisibleRemoveFromQueue { get => isvisibleremovefromqueue; set { isvisibleremovefromqueue = value; OnPropertyChanged(nameof(IsVisibleRemoveFromQueue)); } }

        public QueuePopupPageViewModel(QueuePopupPageState queuePopupPageState, Album album = null)
        {
            
            this.album = album;
            switch (queuePopupPageState)
            {
                case QueuePopupPageState.AddtoAlbumState:
                    Header = "Добавить в альбом";
                    IsVisibleAddToAlbum = true;
                    SetQueue();
                    IsVisibleRemoveFromQueue = false;
                    break;
                case QueuePopupPageState.AddToQueueState:
                    Header = "Список воспроизведения";
                    Queue = DependencyService.Get<IAudio>().Tracks;
                    IsVisibleAddToAlbum = false;
                    IsVisibleRemoveFromQueue = true;
                    break;
            }
            OnPropertyChanged(nameof(QueuePopupPageViewModel));
        }

        private async void SetQueue()
        {
            Queue = new ObservableCollection<Track>(await App.Connections.BaseDataBaseService.DataBase.Table<Track>().ToListAsync());
        }
    }
}
