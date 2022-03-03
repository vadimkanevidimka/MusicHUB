using MusicHUB.Models;
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
    public partial class CutPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public CutPopupPage(Track trackToTrim)
        {
            InitializeComponent();
            BindingContext = new CutPopupPageViewModel(trackToTrim);
        }
    }
}