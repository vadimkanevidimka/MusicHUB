using MusicHUB.Interfaces;
using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.ViewModels.ContextActions
{
    class SetAsRingtoneContextAction : BaseContextAction
    {
        public override string ImageURl => "telephone.png";

        public override string DescriptionText => "Установить на звонок";

        public override void ExcecuteAction<T>(object someobject)
        {
            if (someobject == null)
            {
                return;
            }

            if (someobject.GetType() == typeof(Track))
            {
                Track track = someobject as Track;
                DependencyService.Get<IAndroidSystem>().SetAsRingtone(track.Uri);
                base.MakeToast($"{((Track)someobject).Title} установлена в качестве рингтона.");
            }
        }
    }
}
