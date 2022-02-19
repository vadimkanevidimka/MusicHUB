using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
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
    public partial class TrackLirycsPopupPage : ContentPage
    {
        public TrackLirycsPopupPage()
        {
            InitializeComponent();
            this.BackgroundColor = new Color(0, 0, 0, 0.4);
            BindingContext = this;
        }

        public string Lirycs { get; set; } 

        public ICommand ClosePopupPage
        {
            get => new AsyncCommand(async () => await App.Current.MainPage.Navigation.PopModalAsync());
        }
    }
}