﻿using Android.Widget;
using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.ViewModels.ContextActions
{
    class AddToQueueContextAction : BaseContextAction
    {
        public override string ImageURl => "addtoplaylist.png";

        public override string DescriptionText => "Играть далее";

        public override void ExcecuteAction<T>(object someobject)
        {
            if (someobject == null)
            {
                return;
            }

            if (someobject.GetType() == typeof(Track))
            {
                Track track = someobject as Track;
                var newtrack = track.MemberWiseClone();
                newtrack.Id = DependencyService.Get<IAudio>().Tracks.Last().Id + 1;
                DependencyService.Get<IAudio>().AddToQueue(newtrack);
                base.MakeToast($"{((Track)someobject).Title} добавлена в очередь.");
            }
        }
    }
}
