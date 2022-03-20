using Android.Widget;
using MusicHUB.Models;
using MusicHUB.Pages;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.ViewModels.ContextActions
{
    class QueueContextAction : BaseContextAction
    {
        public override string ImageURl => "list.png";

        public override string DescriptionText => "Список воспроизведения";

        public override async void ExcecuteAction<T>(object someobject)
        {
            if (someobject == null)
            {
                return;
            }

            if (someobject.GetType() == typeof(Track))
            {
                await App.Current.MainPage.Navigation.PopPopupAsync();
                await App.Current.MainPage.Navigation.PushPopupAsync(new QueuePopupPage(Interfaces.QueuePopupPageState.AddToQueueState));
                base.MakeToast($"{((Track)someobject).Title} добавлена в очередь.");
            }
        }
    }
}
