using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MusicHUB.Models;
using System.Windows.Input;
using System.IO;

namespace MusicHUB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpPageFromBottom : Rg.Plugins.Popup.Pages.PopupPage
    {
        public List<ContextAction> contextActions { get; set; }
        private Track Tracks { get; set; }
        public PopUpPageFromBottom(Track track)
        {
            InitializeComponent();
            BindingContext = this;
            contextActions = new List<ContextAction>()
            {
                new ContextAction() {Image = "deletebutton.png", ActionText = "Delete track" },
                new ContextAction() {Image = "downloadbutton.png", ActionText = "Download track" },
                new ContextAction() {Image = "equalizer.png", ActionText = "Equalizer" },
                new ContextAction() {Image = "text.png", ActionText = "Find lirycs" },
            };
            Tracks = track;
            Title.Text = Tracks.Title;
            Artist.Text = Tracks.Artist;
            AudioImg.Source = Tracks.ImageSource;
        }

        public ICommand Delete_click
        {
            get => new Command(() => File.Delete(Tracks.Uri));
        }
    }
}