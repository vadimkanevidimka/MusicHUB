using Android.Content;
using Android.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.Interfaces
{
    public interface IAndroidSystem
    {
        Context AppContext { get; }
        void SetAsRingtone(string url);
    }
}
