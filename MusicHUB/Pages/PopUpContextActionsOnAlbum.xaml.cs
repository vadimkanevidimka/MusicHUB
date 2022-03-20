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

namespace MusicHUB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpContextActionsOnAlbum : Rg.Plugins.Popup.Pages.PopupPage
    {
        public IEnumerable<IContextAction> ContextActions { get; set; }
        public Album Album { get => album; set { album = value; OnPropertyChanged(nameof(Album)); } }
        private int pageHeight;
        public int PageHeight { get => pageHeight; set { pageHeight = value; OnPropertyChanged(nameof(PageHeight)); } }
        public PopUpContextActionsOnAlbum(Album album, TrackContextActionState contextActionState)
        {
            InitializeComponent();
            switch (contextActionState)
            {
                case TrackContextActionState.AtList:
                    ContextActions = new ContextActionsResourses().AlbumListContextActions;
                    break;
                case TrackContextActionState.AtPlayerPage:
                    ContextActions = new ContextActionsResourses().AlbumPageContextActions;
                    break;
                default:
                    break;
            }
            Album = album;
            BindingContext = this;
            Actions.HeightRequest = ContextActions.Count() * 40;
            Pageframe.HeightRequest = Actions.HeightRequest + 120;
        }

        private Album album;

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            IContextAction contextAction = e.Item as IContextAction;
            contextAction.ExcecuteAction<Album>(Album);
            await this.Navigation.PopPopupAsync();
        }

        private ICommand GetSongInfo
        {
            get => new AsyncCommand(async () =>
            {

            });
        }
    }
}