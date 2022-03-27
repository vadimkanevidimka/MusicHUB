using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MusicHUB.ViewModels.ContextActions
{
    class ShareContextAction : BaseContextAction
    {
        public override string ImageURl => "backarrow.png";

        public override string DescriptionText => "Поделиться";

        public override async void ExcecuteAction<T>(object someobject, Action outAction = null)
        {
            if (someobject == null)
            {
                return;
            }

            if (someobject.GetType() == typeof(Track))
            {
                Track track = someobject as Track;
                var newtrack = track.MemberWiseClone();
                await Share.RequestAsync(new ShareFileRequest { Title = track.Title, File = new ShareFile(track.Uri) });
                base.MakeToast($"{((Track)someobject).Title} отправлена.");
            }
        }
    }
}
