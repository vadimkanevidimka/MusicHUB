using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using MusicHUB.Pages;
using System.Collections.ObjectModel;
using MusicHUB.Interfaces;
using System.Threading;
using System.Windows.Input;
using MusicHUB.ViewModels;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    public partial class Player : ContentPage
    {
        public Player()
        {
            InitializeComponent();
            BindingContext = new PlayerViewModel(OpenOptions);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public void OpenOptions()
        {
            this.Navigation.PushPopupAsync(new PopUpPageFromBottom(DependencyService.Get<IAudio>().GetCurrentTrack()));
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
    }
}
