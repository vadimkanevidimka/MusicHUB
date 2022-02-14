using MusicHUB.Models;
using MusicHUB.Pages;
using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MusicHUB.ViewModels
{
    class AlbumPageViewModel : BindableObject
    {
        private List<Track> albumTracks;
        private Album currentAlbum;

        public AlbumPageViewModel(Album currentAlbum)
        {
            CurrentAlbum = currentAlbum;
            UpdateData(CurrentAlbum.Id);
        }

        private Album CurrentAlbum { get => currentAlbum; set { currentAlbum = value; OnPropertyChanged(nameof(CurrentAlbum)); } }

        public List<Track> AlbumTracks { get => albumTracks; set { albumTracks = value; OnPropertyChanged(nameof(AlbumTracks)); } }

        public ImageSource AlbumImage { get => ImageSource.FromStream(() => new MemoryStream(CurrentAlbum.Image)); }

        private async void UpdateData(int albumid)
        {
            AlbumTracks = await App.Connections.BaseDataBaseService.DataBase.QueryAsync<Track>("select * from Tracks join AlbumsTracks on Tracks.Id = TrackId and AlbumId = ?", albumid.ToString());
        }

        public ICommand AddTracksToPlayList
        {
            get => new AsyncCommand(async () => { await App.Current.MainPage.Navigation.PushPopupAsync(new QueuePopupPage()); });
        }
    }
}
