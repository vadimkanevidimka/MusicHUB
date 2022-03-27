using Genius.Models.Artist;
using Genius.Models.Response;
using MusicHUB.DependencyInjection;
using MusicHUB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArtistPage : ContentPage
    {
        public ArtistPage(Artist response)
        {
            InitializeComponent();
            BindingContext = new ArtistPageViewModel(response);
        }
    }
}