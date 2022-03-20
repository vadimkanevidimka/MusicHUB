using MusicHUB.Models;
using MusicHUB.Pages;
using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MusicHUB.ViewModels
{
    class AlbumPageViewModel : BindableObject
    {
        public AlbumPageViewModel(Album currentAlbum)
        {
            CurrentAlbum = currentAlbum;
            UpdateData(CurrentAlbum.Id);
        }

        private List<Track> albumTracks;
        public List<Track> AlbumTracks { get => albumTracks; set { albumTracks = value; OnPropertyChanged(nameof(AlbumTracks)); } }

        public string Title { get => CurrentAlbum.Title; }

        public string Artist { get => CurrentAlbum.Artist; }

        public ImageSource AlbumImage { get => ImageSource.FromStream(() => new MemoryStream(CurrentAlbum.Image)); }

        public ICommand AddTracksToPlayList
        {
            get => new AsyncCommand(async () => { await App.Current.MainPage.Navigation.PushPopupAsync(new QueuePopupPage(Interfaces.QueuePopupPageState.AddtoAlbumState, CurrentAlbum)); });
        }


        private Album currentAlbum;
        private Album CurrentAlbum { get => currentAlbum; set { currentAlbum = value; OnPropertyChanged(nameof(CurrentAlbum)); } }

        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); } }

        private async void UpdateData(int albumid)
        {
            IsRefreshing = true;
            await Task.Delay(1000);
            await GetData(CurrentAlbum.Id);
            IsRefreshing = false;
        }

        private async Task GetData(int albumid)
        {
            AlbumTracks = await App.Connections.BaseDataBaseService.DataBase.QueryAsync<Track>("select * from Tracks join AlbumsTracks on Tracks.Id = TrackId and AlbumId = ?", albumid.ToString());
        }

        public ICommand RefreshCommand
        {
            get => new AsyncCommand(async () =>
            {
                UpdateData(CurrentAlbum.Id);
            });
        }

        public ICommand OptionsClicked
        {
            get => new AsyncCommand<Track>(async (track) => 
            {
               await App.Current.MainPage.Navigation.PushPopupAsync(new PopUpContextActionsOnTrack(track, ContextActions.TrackContextActionState.AtAlbumPage));
            });
        }

        public ICommand AddMusicCommand
        {
            get => new AsyncCommand(async () =>
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new QueuePopupPage(Interfaces.QueuePopupPageState.AddtoAlbumState, CurrentAlbum));
            });
        }

        public ICommand AlbumOptionsClick
        {
            get => new AsyncCommand(async () =>
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new PopUpContextActionsOnAlbum(CurrentAlbum, ContextActions.TrackContextActionState.AtPlayerPage));
            });
        }

        public ICommand PlayNextCommand
        {
            get => new AsyncCommand(async () =>
            {
                DependencyService.Get<IAudio>().AddToQueue(AlbumTracks);
            });
        }

        public ICommand TrackClicked
        {
            get => new AsyncCommand<Track>(async (track) =>
            {
                DependencyService.Get<IAudio>().SetQueue(AlbumTracks);
                await DependencyService.Get<IAudio>().PlayAudioFile(track);
            });
        }
    }
}
