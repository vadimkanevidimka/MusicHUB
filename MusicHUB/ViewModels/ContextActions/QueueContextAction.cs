using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.ViewModels.ContextActions
{
    class QueueContextAction : IContextAction
    {
        public string ImageURl => "list.png";

        public string DescriptionText => "Список воспроизведения";

        public void ExcecuteAction<T>(object someobject)
        {
            var items = DependencyService.Get<IAudio>().GetCurrentTrack();
        }
    }
}
