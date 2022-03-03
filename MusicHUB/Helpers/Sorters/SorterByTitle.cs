using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MusicHUB.Interfaces;

namespace MusicHUB.Helpers
{
    public enum OrderType
    {
        Descending,
        Ascending,
    }

    public class SorterByTitle : ISorter
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
                case OrderType.Ascending: return tracks.OrderBy((c) => c.Title).ToList();
                case OrderType.Descending: return tracks.OrderByDescending((c) => c.Title).ToList();
                default: return tracks.OrderBy((c) => c.Title).ToList();
            }
        }
    }
}
