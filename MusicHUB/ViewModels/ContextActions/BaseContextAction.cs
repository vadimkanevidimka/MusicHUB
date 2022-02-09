using Android.Widget;
using MusicHUB.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.ViewModels.ContextActions
{
    abstract class BaseContextAction : IContextAction
    {
        public abstract string ImageURl { get; }

        public abstract string DescriptionText { get; }

        public abstract void ExcecuteAction<T>(object someobject);

        protected void MakeToast(string text)
        {
            Toast.MakeText(DependencyService.Get<IAndroidSystem>().AppContext, text, ToastLength.Short).Show();
        }
    }
}
