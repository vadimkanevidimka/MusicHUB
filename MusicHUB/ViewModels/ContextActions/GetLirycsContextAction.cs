using MusicHUB.Models;
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

        public override void ExcecuteAction<T>(object someobject)
        {
            if (typeof(T) == typeof(Track))
            {
                var tr = DependencyService.Get<IAudio>().GetCurrentTrack();
            }
        }
    }
}
