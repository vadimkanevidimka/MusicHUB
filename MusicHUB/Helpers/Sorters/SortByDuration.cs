using MusicHUB.Interfaces;
using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHUB.Helpers.Sorters
{
    class SortByDuration : ISorter
    {
        public async Task<IList<Track>> SortAsync(IEnumerable<Track> tracks, OrderType orderType)
        {
            return await Task.Run(() => Sort(tracks, orderType));
        }

        public IList<Track> Sort(IEnumerable<Track> tracks, OrderType orderType)
        {
            if (tracks is null)
            {
                throw new ArgumentNullException(nameof(tracks));
            }

            switch (orderType)
            {
                case OrderType.Ascending: return tracks.OrderBy((c) => c.Duration).ToList();
                case OrderType.Descending: return tracks.OrderByDescending((c) => c.Duration).ToList();
                default: return tracks.OrderBy((c) => c.Duration).ToList();
            }
        }
    }
}
