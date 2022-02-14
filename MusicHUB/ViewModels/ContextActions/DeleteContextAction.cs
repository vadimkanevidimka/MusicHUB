using Android.Content;
using Android.Provider;
using Android.Widget;
using MusicHUB.Interfaces;
using MusicHUB.Models;
using System.IO;
using Xamarin.Forms;

namespace MusicHUB.ViewModels.ContextActions
{
    class DeleteContextAction : BaseContextAction
    {
        public override string ImageURl => "deletebutton.png";
        public override string DescriptionText => "Удалить";

        public override void ExcecuteAction<T>(object someobject)
        {
            if (someobject == null)
            {
                return;
            }

            if (typeof(T) == typeof(Track))
            {
                Track track = someobject as Track;
                DependencyService.Get<IAudio>().Pause();
                if (DependencyService.Get<IFileProvider>().DeleteFile(track.Uri))
                {
                    base.MakeToast($"{track.Title} удален");
                    DependencyService.Get<IAudio>().RemoveFromQueue(track);
                    DependencyService.Get<IAudio>().Next();
                }
                base.MakeToast($"{track.Title} не удален");
            }
            else
            {

            }
        }
    }
}
