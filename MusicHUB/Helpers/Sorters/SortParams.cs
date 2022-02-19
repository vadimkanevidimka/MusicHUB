using MusicHUB.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.Helpers.Sorters
{
    public class SortParams
    {
        public ISorter Sorter { get; set; }
        public OrderType OrderType { get; set; }
    }
}
