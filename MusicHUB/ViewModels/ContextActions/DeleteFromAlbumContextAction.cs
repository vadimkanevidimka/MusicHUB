using MusicHUB.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.ViewModels.ContextActions
{
    class DeleteFromAlbumContextAction : BaseContextAction
    {
        public override string ImageURl => "deletebutton.png";

        public override string DescriptionText => "Удалить из плейлиста";

        public override async void ExcecuteAction<T>(object someobject)
        {
            if (someobject == null)
            {
                return;
            }

            if (someobject.GetType() == typeof(AlbumsTracks))
            {
                AlbumsTracks track = someobject as AlbumsTracks;

                await App.Current.MainPage.Navigation.PopPopupAsync();
                await App.Connections.BaseDataBaseService.DataBase.QueryAsync<DBLikedTracks>("delete where AlbumId = ? and TrackId = ?", new int[] { track.AlbumId, track.TrackId });
                base.MakeToast($"Композиция удвлена из плейлиста");
            }
        }
    }
}
