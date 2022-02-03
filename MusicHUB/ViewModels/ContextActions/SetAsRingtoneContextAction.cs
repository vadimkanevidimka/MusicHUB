using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.ViewModels.ContextActions
{
    class SetAsRingtoneContextAction : IContextAction
    {
        public string ImageURl => "sound.png";

        public string DescriptionText => "Установить на звонок";

        public void ExcecuteAction<T>(object someobject)
        {
            
        }
    }
}
