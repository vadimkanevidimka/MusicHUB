using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.CustomElements
{
    class WheatPicker : Picker
    {
        public WheatPicker() : base()
        {
            BackgroundColor = Color.Wheat;
            base.TextColor = Color.Black;
        }
    }
}
