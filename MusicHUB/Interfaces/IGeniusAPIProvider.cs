using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicHUB.Interfaces
{
    public interface IGeniusAPIProvider
    {
        Task<T> Getdata<T>(Dictionary<string, string> parameters);
    }
}
