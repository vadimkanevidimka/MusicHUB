using MusicHUB.Models;
using MusicHUB.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.ViewModels.ContextActions
{
    class EditMetaDataContextAction : BaseContextAction
    {
        public override string ImageURl => "tracktext.png";

        public override string DescriptionText => "Изменить данные";

        public async override void ExcecuteAction<T>(object someobject)
        {

            if (someobject == null)
            {
                return;
            }

            if (someobject.GetType() == typeof(Track))
            {
                if (App.Current.MainPage.Navigation.ModalStack.Count == 1)
                {
                    await App.Current.MainPage.Navigation.PopModalAsync();
                }
                await App.Current.MainPage.Navigation.PushAsync(new EditMusicTagsPopupPage((Track)someobject));
            }
            else if (someobject.GetType() == typeof(Album))
            {
                
            }
        }
    }
}
