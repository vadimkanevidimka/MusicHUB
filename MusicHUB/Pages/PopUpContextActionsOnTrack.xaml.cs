using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MusicHUB.Models;
using System.Windows.Input;
using System.IO;
using Android.Widget;
using MusicHUB.ViewModels.ContextActions;
using MvvmHelpers.Commands;
using MusicHUB.Pages;
using Xamarin.Essentials;

namespace MusicHUB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpContextActionsOnTrack : Rg.Plugins.Popup.Pages.PopupPage
    {
        public IEnumerable<IContextAction> ContextActions { get; set; }
        public Track Tracks { get; set; }
        private int pageHeight;
        public int PageHeight { get => pageHeight; set { pageHeight = value; OnPropertyChanged(nameof(PageHeight)); } }

        public PopUpContextActionsOnTrack(Track track, TrackContextActionState contextActionState)
        {
            InitializeComponent();
            switch (contextActionState)
            {
                case TrackContextActionState.AtList:
                    ContextActions = new ContextActionsResourses().TrackListContextActions;
                    break;
                case TrackContextActionState.AtPlayerPage:
                    ContextActions = new ContextActionsResourses().TrackContextActions;
                    break;
                case TrackContextActionState.AtAlbumPage:
                    ContextActions = new ContextActionsResourses().TrackInAlbumListContextActions;
                    break;
                default:
                    break;
            }
            Tracks = track;
            BindingContext = this;
            Actions.HeightRequest = ContextActions.Count() * 40;
            Pageframe.HeightRequest = Actions.HeightRequest + 140;
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            IContextAction contextAction = e.Item as IContextAction;
            contextAction.ExcecuteAction<Track>(Tracks);
            await this.Navigation.PopPopupAsync();
        }

        public ICommand GetSongInfo
        {
            get => new AsyncCommand(async () =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    await App.Current.MainPage.Navigation.PopPopupAsync();
                    if (App.Current.MainPage.Navigation.ModalStack.Count > 0)
                    {
                        await App.Current.MainPage.Navigation.PopModalAsync();
                    }
                    await App.Current.MainPage.Navigation.PushAsync(new TrackPage(0, Tracks.Artist, Tracks.Title));
                }
                else
                {
                    App.Message.SetText("Отсутствует подключение к интернету");
                    App.Message.Duration = 0;
                    App.Message.Show();
                }
            });
        }
    }
}