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

        public override async void ExcecuteAction<T>(object someobject, Action outAction = null)
        {
            if (someobject == null)
            {
                return;
            }

            if (someobject.GetType() == typeof(Track))
            {
                Track track = someobject as Track;

                await App.Current.MainPage.Navigation.PopPopupAsync();
                await App.Connections.BaseDataBaseService.DataBase.QueryAsync<AlbumsTracks>("delete from AlbumsTracks where TrackId = ?", track.Id);
                base.MakeToast($"Композиция удвлена из плейлиста");
            }
        }
    }
}
