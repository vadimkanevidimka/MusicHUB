using MusicHUB.Models;
using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Extensions;
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
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public ICommand ClearDataBaseCommand
        {
            get => new AsyncCommand(async () =>
            {
                bool result = await DisplayAlert("Очистка базы данных", "Хотите очистить базу данных?", "да", "нет");
                if (result)
                {
                    await App.Connections.BaseDataBaseService.DataBase.DeleteAllAsync<DBLikedTracks>();
                    await App.Connections.BaseDataBaseService.DataBase.DeleteAllAsync<Album>();
                    await App.Connections.BaseDataBaseService.DataBase.DeleteAllAsync<AlbumsTracks>();
                    await App.Connections.BaseDataBaseService.DataBase.DeleteAllAsync<RecentlyPlayed>();
                    await App.Connections.BaseDataBaseService.DataBase.DeleteAllAsync<Track>();
                }

            });
        }

        public ICommand SendEmailCommand
        {
            get => new AsyncCommand(async () =>
            {
                await Xamarin.Essentials.Email.ComposeAsync("Сообщить о проблеме в приложении или идеях для улучшения пользовательского опыта.", string.Empty, new string[] { "vadimdeg6@gmail.com" });
            });
        }

        public ICommand RateUsCommand
        {
            get => new AsyncCommand(async () =>
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new RatePopupPage());
            });
        }

        public ICommand AppInfoCommand
        {
            get => new AsyncCommand(async () =>
            {
                await DisplayAlert("Справка о системе", "Приложение разаработал:\nДегтярюк Вадим Дмитриевич\n2022 ","Ок");
            });
        }

        public ICommand AppTutorialCommand
        {
            get => new AsyncCommand(async () =>
            {
                await Browser.OpenAsync("https://youtu.be/kaZJeyBTSgo");
            });
        }
    }
}