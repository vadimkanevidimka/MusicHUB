using MusicHUB.Models;
using System.IO;

namespace MusicHUB.ViewModels.ContextActions
{
    class DeleteContextAction : IContextAction
    {
        public string ImageURl => "deletebutton.png";
        public string DescriptionText => "Удалить";

        public void ExcecuteAction<T>(object someobject)
        {
            if (typeof(T) == typeof(Track))
            {
                File.Delete(((Track)someobject).Uri);
            }
            else
            {

            }
        }
    }
}
