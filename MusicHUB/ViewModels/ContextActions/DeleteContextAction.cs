using Android.Widget;
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
                DependencyService.Get<IAudio>().Pause();
                File.Delete(((Track)someobject).Uri);
                base.MakeToast($"Композиция {((Track)someobject).Title} удалена");
            }
            else
            {

            }
        }
    }
}
