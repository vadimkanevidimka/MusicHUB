using MusicHUB.Helpers;
using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicHUB.Interfaces
{
    public interface ISorter
    {
        public IList<Track> Sort(IEnumerable<Track> tracks, OrderType orderType);
        public Task<IList<Track>> SortAsync(IEnumerable<Track> tracks, OrderType orderType);
    }
}
