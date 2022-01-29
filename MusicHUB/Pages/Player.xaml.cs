using MusicHUB.Models;
using System;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using MusicHUB.ViewModels;
using MusicHUB.DependencyInjection;
using System.Windows.Input;

namespace MusicHUB.Pages
{
    public partial class Player : ContentPage
    {
        public Player(Connections connections)
        {
            InitializeComponent();
            BindingContext = new PlayerViewModel(Navigation, connections);
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

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
