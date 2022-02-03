using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.ViewModels.ContextActions
{
    class GetLirycsContextAction : IContextAction
    {
        public string ImageURl => "text.png";
        public string DescriptionText => "Найти текст песни";

        public void ExcecuteAction<T>(object someobject)
        {
            if (typeof(T) == typeof(Track))
            {
                var tr = DependencyService.Get<IAudio>().GetCurrentTrack();
            }
        }
    }
}
