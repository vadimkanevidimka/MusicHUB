﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.ViewModels
{
    public class ResourceClassPlayerPage
    {
        internal const string PlayImg = "Play.png";
        internal const string PauseImg = "Pause.png";
        internal const string ShufleImg = "shuffle_non_active.png";
        internal const string ShuffledImg = "shuffle_active.png";
        internal const string LoopImg = "Repeat.png";
        internal const string LoopedImg = "RepeatOnce.png";

        internal ResourceClassPlayerPage()
        {
            PlayPause = PauseImg;
            Shufle = ShufleImg;
            Repeat = LoopImg;
        }

        public string PlayPause { get; set; }
        public string Shufle { get; set; }
        public string Repeat { get; set; }
    }
}