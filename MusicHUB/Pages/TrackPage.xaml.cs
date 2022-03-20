﻿using Genius.Models.Artist;
using Genius.Models.Response;
using Genius.Models.Song;
using MusicHUB.DependencyInjection;
using MusicHUB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackPage : ContentPage
    {
        public TrackPage(ulong id, string artist = null, string title = null)
        {
            InitializeComponent();

            BindingContext = new TrackPageViewModel(id, artist, title);
        }
    }
}