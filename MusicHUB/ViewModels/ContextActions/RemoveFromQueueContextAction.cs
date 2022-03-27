using Android.Widget;
using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.ViewModels.ContextActions
{
    class RemoveFromQueueContextAction : BaseContextAction
    {
        public override string ImageURl => "removefromQueue.png";

        public override string DescriptionText => "Удалить из очереди";

        public override void ExcecuteAction<T>(object someobject, Action outAction = null)
        {
            if (someobject == null)
            {
                return;
            }

            if (someobject.GetType() == typeof(Track))
            {
                Track track = someobject as Track;
                DependencyService.Get<IAudio>().RemoveFromQueue(track);
                base.MakeToast($"{((Track)someobject).Title} удалена из очереди.");
            }
        }
    }
}
