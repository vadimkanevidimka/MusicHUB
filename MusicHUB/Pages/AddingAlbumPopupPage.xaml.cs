using MusicHUB.Interfaces;
using MusicHUB.Models;
using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingAlbumPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public AddingAlbumPopupPage()
        {
            InitializeComponent();
            BindingContext = this;
            NewAlbum = new Album();
        }

        public Album NewAlbum { get; set; }

        public ICommand AddNewAlbumCommand
        {
            get => new AsyncCommand(async () =>
            {
                if (NewAlbum is null) return;
                await App.Connections.BaseDataBaseService.DataBase.InsertAsync(NewAlbum);
                await App.Current.MainPage.Navigation.PopPopupAsync(); 
            });
        }

        public ICommand ChoseAlbumPic
        {
            get => new AsyncCommand(async () =>
            {
                NewAlbum.Image = await DependencyService.Get<IFileProvider>().PickImageFromFiles(PickOptions.Images);
            });
        }
    }
}