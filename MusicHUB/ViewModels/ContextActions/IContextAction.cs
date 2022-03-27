using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.ViewModels.ContextActions
{
    public interface IContextAction
    {
        public string ImageURl { get; }
        public string DescriptionText { get; }
        public void ExcecuteAction<T>(object someobject, Action action = null);
    }
}
