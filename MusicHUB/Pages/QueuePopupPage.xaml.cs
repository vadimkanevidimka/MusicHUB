using Genius.Models.Song;
using MusicHUB.Interfaces;
using MusicHUB.ViewModels;
using Rg.Plugins.Popup.Pages;
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
    public partial class QueuePopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public QueuePopupPage(QueuePopupPageState queuePopupPageState, MusicHUB.Models.Album album = null)
        {
            InitializeComponent();
            BindingContext = new QueuePopupPageViewModel(queuePopupPageState, album);
        }
    }
}