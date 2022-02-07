using Genius;
using MusicHUB.DataBaseServices;
using MusicHUB.Interfaces;

namespace MusicHUB.DependencyInjection
{
    public class Connections
    {
        public Connections(DataBaseService dataBaseService, GeniusClient geniusClient)
        {
            BaseDataBaseService = dataBaseService;
            GeniusClient = geniusClient;
        }

        public IDataBaseService BaseDataBaseService { get; }

        public GeniusClient GeniusClient { get; }
    }
}
