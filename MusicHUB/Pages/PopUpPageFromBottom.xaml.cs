using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MusicHUB.Models;

namespace MusicHUB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpPageFromBottom : Rg.Plugins.Popup.Pages.PopupPage
    {
        private Track Tracks { get; set; }
        public PopUpPageFromBottom(Track track)
        {
            InitializeComponent();
            Tracks = track;
            Title.Text = Tracks.Title;
            Artist.Text = Tracks.Artist;
            AudioImg.Source = Tracks.ImageSource;

        }
    }
}