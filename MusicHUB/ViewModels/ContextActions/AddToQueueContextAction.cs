using Android.Widget;
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
        public override string ImageURl => "playlist1.png";

        public override string DescriptionText => "Играть далее";

        public async override void ExcecuteAction<T>(object someobject)
        {
            if (someobject == null)
            {
                return;
            }

            if (someobject.GetType() == typeof(Track))
            {
                Track track = someobject as Track;
                var newtrack = track.MemberWiseClone();
                newtrack.UniquePosition = DependencyService.Get<IAudio>().Tracks.Count + 1;
                DependencyService.Get<IAudio>().AddToQueue(newtrack);
                base.MakeToast($"{((Track)someobject).Title} добавлена в очередь.");
            }
            else if (someobject.GetType() == typeof(Album))
            {
                Album album = someobject as Album;
                var tracks = await App.Connections.BaseDataBaseService.DataBase.QueryAsync<Track>("select * from Tracks join AlbumsTracks on Tracks.Id = AlbumsTracks.TrackId");
                DependencyService.Get<IAudio>().AddToQueue(tracks);
                base.MakeToast($"{((Album)someobject).Title} добавлена в очередь.");
            }
        }
    }
}
