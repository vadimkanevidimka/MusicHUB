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
        public AlbumPageViewModel(Album currentAlbum)
        {
            CurrentAlbum = currentAlbum;
            UpdateData(CurrentAlbum.Id);
        }

        public List<Track> AlbumTracks { get => albumTracks; set { albumTracks = value; OnPropertyChanged(nameof(AlbumTracks)); } }

        public string Title { get => CurrentAlbum.Title; }

        public string Artist { get => CurrentAlbum.Artist; }

        public ImageSource AlbumImage { get => ImageSource.FromStream(() => new MemoryStream(CurrentAlbum.Image)); }

        public ICommand AddTracksToPlayList
        {
            get => new AsyncCommand(async () => { await App.Current.MainPage.Navigation.PushPopupAsync(new QueuePopupPage()); });
        }

        private List<Track> albumTracks;

        private Album currentAlbum;

        private Album CurrentAlbum { get => currentAlbum; set { currentAlbum = value; OnPropertyChanged(nameof(CurrentAlbum)); } }

        private async void UpdateData(int albumid)
        {
            AlbumTracks = await App.Connections.BaseDataBaseService.DataBase.QueryAsync<Track>("select * from Tracks join AlbumsTracks on Tracks.Id = TrackId and AlbumId = ?", albumid.ToString());
        }

    }
}
