using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.Interfaces
{
    public interface ICreateTrack
    {
        Track CreateTrack(string filename);
    }
}
