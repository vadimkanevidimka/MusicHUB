using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.Pages
{
    public class PlayerCarouselDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Tab1 { get; set; }
        public DataTemplate Tab2 { get; set; }

        public PlayerCarouselDataTemplateSelector()
        {
            Tab1 = new DataTemplate(typeof(PlayerPage));
            Tab2 = new DataTemplate(typeof(TrackList));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var position = $"{item}";

            if (position == "1")
                return Tab1;

            return Tab2;
        }
    }
}
