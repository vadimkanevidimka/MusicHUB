using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.EventArgumentss
{
    public class PlayerTimeEventArgs : EventArgs
    {
        public int CurrentTime { get; private set; }
        public int Duration { get; private set; }

        public PlayerTimeEventArgs(int currenttime, int duration)
        {
            CurrentTime = currenttime;
            Duration = duration;
        }
    }
}
