﻿using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MusicHUB.Models;
using System.Windows.Input;
using System.IO;
using Android.Widget;
using MusicHUB.ViewModels.ContextActions;

namespace MusicHUB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpContextActionsOnTrack : Rg.Plugins.Popup.Pages.PopupPage
    {
        public IEnumerable<IContextAction> ContextActions { get; set; }
        public Track Tracks { get; set; }
        public PopUpContextActionsOnTrack(Track track, TrackContextActionState contextActionState)
        {
            InitializeComponent();
            switch (contextActionState)
            {
                case TrackContextActionState.AtList:
                    ContextActions = new ContextActionsResourses().TrackListContextActions;
                    break;
                case TrackContextActionState.AtPlayerPage:
                    ContextActions = new ContextActionsResourses().TrackContextActions;
                    break;
                default:
                    break;
            }
            Tracks = track;
            BindingContext = this;
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            IContextAction contextAction = e.Item as IContextAction;
            contextAction.ExcecuteAction<Track>(Tracks);
            await this.Navigation.PopPopupAsync();
        }
    }
}