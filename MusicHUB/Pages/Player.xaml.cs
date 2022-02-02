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
                Close_Clicked(this, new EventArgs());
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void Slider_DragCompleted(object sender, EventArgs e)
        {
            Slider slider = sender as Slider;
            DependencyService.Get<IAudio>().SetTime((int)slider.Value);
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}
