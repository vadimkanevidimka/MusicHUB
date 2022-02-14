using MusicHUB.Models;
using MusicHUB.Pages;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.ViewModels.ContextActions
{
    class AddToPlayListContextAction : BaseContextAction
    {
        public override string ImageURl => "library.png";

        public override string DescriptionText => "Добавить в плейлист";

        public override async void ExcecuteAction<T>(object someobject)
        {
            if (someobject == null)
            {
                return;
            }

            if (someobject.GetType() == typeof(Track))
            {
                Track track = someobject as Track;
                var newtrack = track.MemberWiseClone();
                await App.Current.MainPage.Navigation.PopPopupAsync();
                await App.Current.MainPage.Navigation.PushPopupAsync(new ChosePlayListPopupPage(track));
                base.MakeToast($"{((Track)someobject).Title} добавлена в плейлист.");
            }
        }
    }
}
