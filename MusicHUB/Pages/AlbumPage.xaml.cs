using MusicHUB.Models;
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
    public partial class AlbumPage : ContentPage
    {
        public AlbumPage(Album currentAlbum)
        {
            InitializeComponent();
            BindingContext = new AlbumPageViewModel(currentAlbum);
        }
    }
}