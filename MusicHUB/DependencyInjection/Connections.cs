using Genius;
using MusicHUB.DataBaseServices;

namespace MusicHUB.DependencyInjection
{
    public class Connections
    {
        public Connections(DataBaseService dataBaseService, GeniusClient geniusClient)
        {
            BaseDataBaseService = dataBaseService;
            GeniusClient = geniusClient;
        }

        public BaseDataBaseService BaseDataBaseService { get; }

        public GeniusClient GeniusClient { get; }
    }
}
