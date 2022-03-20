using MusicHUB.Helpers;
using MusicHUB.Helpers.Sorters;
using MusicHUB.Interfaces;
using MusicHUB.ViewModels.ContextActions;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatePopupPage : ContentPage
    {
        private int rating;
        public int Rating 
        { 
            get => rating;
            set 
            {
                rating = value;
                SetSmile(rating);
                OnPropertyChanged(nameof(Rating));
            } 
        }

        private string image;
        public string Image { get => image; set { image = value; OnPropertyChanged(nameof(Image)); } }

        public RatePopupPage()
        {
            InitializeComponent();
            BindingContext = this;
            Rating = 1;
        }

        private void SetSmile(int rate)
        {
            switch (rate)
            {
                case 1:
                    Image = "crying.png";
                    break;
                case 2:
                    Image = "confused.png";
                    break;
                case 3:
                    Image = "neutral.png";
                    break;
                case 4:
                    Image = "smile.png";
                    break;
                case 5:
                    Image = "famous.png";
                    break;
            }
        }

        public ICommand ClosePopupPage
        {
            get => new AsyncCommand(async () =>
            {
                await App.Current.MainPage.Navigation.PopModalAsync();
            });
        }

        public ICommand SendEmailCommand
        {
            get => new AsyncCommand(async () =>
            {
                await Xamarin.Essentials.Email.ComposeAsync($"Я оценил приложение MusicHUB на {Rating}", "", new string[] { "vadimdeg6@gmail.com" });
            });
        }
    }
}