﻿using MusicHUB.DataBaseServices;
using MusicHUB.Interfaces;
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
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage(IDataBaseService dataBaseService)
        {
            InitializeComponent();
            Children.Add(new MainPage(dataBaseService));
            Children.Add(new PlayListsPage(dataBaseService));
        }
    }
}