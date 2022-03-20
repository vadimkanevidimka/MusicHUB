using MusicHUB.DataBaseServices;
using MusicHUB.Interfaces;
using MusicHUB.Models;
using MusicHUB.Pages;
using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MusicHUB.ViewModels
{
    public class PLayListViewModel : BindableObject
    {
        private List<Track> likedTracks;
        private List<Album> albums;
        private List<Track> recentlyPlayed;
        private SQLite.SQLiteAsyncConnection DataBase {get;set;}
        private bool isRefreshing = false;

        public PLayListViewModel()
        {
            Init();
        }

        private async void Init()
        {
            DataBase = await ConnectionCreator.Init();
            await UpdateData();
        }

        public List<Album> Albums
        {
            get => albums;
            set
            {
                albums = value;
                OnPropertyChanged(nameof(Albums));
            }
        }

        public bool IsRefreshing { get => isRefreshing;
            set 
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing)); 
            } 
        }

        public List<Track> LikedTracks { get => likedTracks;
            set
            {
                likedTracks = value;
                OnPropertyChanged(nameof(LikedTracks));
            }
        }

        public List<Track> RecentlyPlayed
        {
            get => recentlyPlayed;
            set
            {
                recentlyPlayed = value;
                OnPropertyChanged(nameof(RecentlyPlayed));
            }
        }

        public ICommand SelectLikedCommand
        {
            get => new AsyncCommand<Track>(async (Track track) => { 
                DependencyService.Get<IAudio>().SetQueue(LikedTracks);
                await Task.Delay(100);
                await DependencyService.Get<IAudio>().PlayAudioFile(track);
            });
        }

        public ICommand RecentlyPlayedCommand
        {
            get => new AsyncCommand<Track>(async (Track track) => {
                DependencyService.Get<IAudio>().SetQueue(RecentlyPlayed);
                await Task.Delay(100);
                await DependencyService.Get<IAudio>().PlayAudioFile(track);
            });
        }

        public ICommand GetTrackInfo
        {
            get => new AsyncCommand<Track>(async (track) =>
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new PopUpContextActionsOnTrack(track, ViewModels.ContextActions.TrackContextActionState.AtList));
            });
        }

        public ICommand RefreshCommand
        {
            get => new AsyncCommand(() => UpdateData());
        }

        private async Task UpdateData()
        {
            IsRefreshing = true;
            try
            {
                await Task.Delay(1000);
                LikedTracks = await DataBase.QueryAsync<Track>("select Tracks.Id, Tracks.Title, Tracks.Artist, Tracks.Album, Tracks.Year, Tracks.Bitrate, Tracks.Uri, Tracks.Duration, Tracks.DurationString from Tracks JOIN LikedTracks on Tracks.Id = TrackId");
                Albums = await App.Connections.BaseDataBaseService.DataBase.Table<Album>().ToListAsync();
                RecentlyPlayed = await App.Connections.BaseDataBaseService.DataBase.QueryAsync<Track>("select * from Tracks join RecentlyPlayed on Tracks.Id = TrackId Limit 20");
                IsRefreshing = false;

            }
            catch (Exception)
            {
                IsRefreshing = false;
            }
        }

        public ICommand AddNewAlbum
        {
            get => new AsyncCommand(() => App.Current.MainPage.Navigation.PushPopupAsync(new AddingAlbumPopupPage()));
        }

        public ICommand SelectAlbum
        {
            get => new AsyncCommand<Album>(async (Album album) =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new AlbumPage(album));
            });
        }

        public ICommand AlbumOptionsCommand
        {
            get => new AsyncCommand<Album>(async (album) => { await App.Current.MainPage.Navigation.PushPopupAsync(new PopUpContextActionsOnAlbum(album, ContextActions.TrackContextActionState.AtList)); });
        }
    }
}
