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
    class ChosePlayListPopupPageViewModel : BindableObject
    {
        private Track TrackToPlayList;
        private IList<Album> albums;

        public ChosePlayListPopupPageViewModel(Track trackToplaylist)
        {
            this.TrackToPlayList = trackToplaylist;
            GetData();
        }

        public IList<Album> Albums { get => albums; set { albums = value; OnPropertyChanged(nameof(Albums)); } }
        public Album SelectedAlbum { get; set; }

        public ICommand AddCommand
        {
            get => new AsyncCommand(async () => {
                if (SelectedAlbum is null) return;
                await App.Connections.BaseDataBaseService.DataBase.InsertOrReplaceAsync(new AlbumsTracks() { AlbumId = SelectedAlbum.Id, TrackId = TrackToPlayList.Id });
                await App.Current.MainPage.Navigation.PopPopupAsync();
            });
        }

        private async void GetData()
        {
            Albums = await App.Connections.BaseDataBaseService.DataBase.Table<Album>().ToListAsync();
        }
    }
}
