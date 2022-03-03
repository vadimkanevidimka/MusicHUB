using MusicHUB.Models;
using MusicHUB.Pages;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.ViewModels.ContextActions
{
    class CutContextAction : BaseContextAction
    {
        public override string ImageURl => "cut.png";

        public override string DescriptionText => "Обрезать";

        public override async void ExcecuteAction<T>(object someobject)
        {
            if (someobject == null)
            {
                return;
            }

            if (someobject.GetType() == typeof(Track))
            {
                await App.Current.MainPage.Navigation.PopPopupAsync();
                App.Current.MainPage.Navigation.PushPopupAsync(new CutPopupPage((Track)someobject));
                base.MakeToast($"{((Track)someobject).Title} обрезана и сохранена в папку /storage/emulated/0/{Android.OS.Environment.DirectoryMusic}/");
            }
        }
    }
}
