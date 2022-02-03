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
using Android.Widget;
using MusicHUB.ViewModels.ContextActions;

namespace MusicHUB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpContextActionsOnTrack : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ContextActionsResourses ContextActions { get; set; }
        public Track Tracks { get; set; }
        public PopUpContextActionsOnTrack(Track track)
        {
            InitializeComponent();
            ContextActions = new ContextActionsResourses();
            Tracks = track;
            BindingContext = this;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            IContextAction contextAction = e.SelectedItem as IContextAction;
            contextAction.ExcecuteAction<Track>(Tracks);
            await this.Navigation.PopPopupAsync();
        }
    }
}