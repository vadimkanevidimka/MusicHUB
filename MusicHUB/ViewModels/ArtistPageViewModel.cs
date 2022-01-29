using Genius.Models.Response;
using MusicHUB.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.ViewModels
{
    public class ArtistPageViewModel
    {
        public ArtistPageViewModel(SearchResponse response, Connections connections)
        {
            this.Response = response;
            this.Connections = connections;
        }

        public SearchResponse Response { get; set; }
        public Connections Connections { get; set; }
    }
}
