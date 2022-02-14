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
    public partial class ChosePlayListPopupPage : PopupPage
    {
        public ChosePlayListPopupPage(Track trackToplaylist)
        {
            InitializeComponent();
            BindingContext = new ChosePlayListPopupPageViewModel(trackToplaylist);
        }
    }
}