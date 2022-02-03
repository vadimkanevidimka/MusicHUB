using Android.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.Interfaces
{
    public interface IAndroidSystem
    {
        RingtoneManager RingtoneManager { get; }
    }
}
