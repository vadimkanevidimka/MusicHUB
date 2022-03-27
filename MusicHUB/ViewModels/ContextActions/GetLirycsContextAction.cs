using MusicHUB.Models;
using MusicHUB.Pages;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.ViewModels.ContextActions
{
    class GetLirycsContextAction : BaseContextAction
    {
        public override string ImageURl => "text.png";
        public override string DescriptionText => "Найти текст песни";

        public override async void ExcecuteAction<T>(object someobject, Action outAction = null)
        {
            if (typeof(T) == typeof(Track))
            {
                Track track = someobject as Track;
                await App.Current.MainPage.Navigation.PopPopupAsync();
                await App.Current.MainPage.Navigation.PushModalAsync(new TrackLirycsPopupPage());
            }
        }
    }
}
