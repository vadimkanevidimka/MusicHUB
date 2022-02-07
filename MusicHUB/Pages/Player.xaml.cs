using MusicHUB.Models;
using System;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using MusicHUB.ViewModels;
using MusicHUB.DependencyInjection;
using System.Windows.Input;
using Android.Widget;

namespace MusicHUB.Pages
{
    public partial class Player : ContentPage
    {
        public Player(Connections connections)
        {
            try
            {
                InitializeComponent();
                BindingContext = new PlayerViewModel(Navigation, connections);

            }
            catch (Exception ex)
            {
                var context = DependencyService.Get<IAudio>().GetContext;
                var toast = Toast.MakeText(context, ex.Message, ToastLength.Long);
                toast.Show();
                this.Navigation.PopModalAsync();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
