using MusicHUB.Helpers;
using MusicHUB.Helpers.Sorters;
using MusicHUB.Interfaces;
using MusicHUB.ViewModels.ContextActions;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SortPopupPage : ContentPage
    {
        public SortPopupPage(SortParams sortParams, Action method)
        {
            InitializeComponent();
            this.BackgroundColor = new Color(0, 0, 0, 0);
            SortActions = new List<string>() { "названию А-Z", "названию Z-A", "исполнителю Z-A", "исполнителю A-z", "длительности (по возраст.)", "длительности (по убыв.)"};
            BindingContext = this;

            if (sortParams != null)
            {
                SortParams = sortParams;
            }
            else
            {
                SortParams = new SortParams();
            }

            this.Disappearing += UpdateList;
            UpdateAction += method;
        }

        private Action UpdateAction;
        private SortParams SortParams;
        public List<string> SortActions { get; set; }

        private async void UpdateList(object sender, EventArgs e)
        {
            if (UpdateAction != null && SortParams.Sorter != null)
            {
                await Task.Run(() => UpdateAction());
            }
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string text = e.Item as string;
            switch (text)
            {
                case "названию А-Z":
                    SortParams.Sorter = new SorterByTitle();
                    SortParams.OrderType = OrderType.Descending;
                    break;
                case "названию Z-A":
                    SortParams.Sorter = new SorterByTitle();
                    SortParams.OrderType = OrderType.Ascending;
                    break;
                case "исполнителю Z-A":
                    SortParams.Sorter = new SortByArtist();
                    SortParams.OrderType = OrderType.Descending;
                    break;
                case "исполнителю A-z":
                    SortParams.Sorter = new SortByArtist();
                    SortParams.OrderType = OrderType.Ascending;
                    break;
                case "длительности (по возраст.)":
                    SortParams.Sorter = new SortByDuration();
                    SortParams.OrderType = OrderType.Ascending;
                    break;
                case "длительности (по убыв.)":
                    SortParams.Sorter = new SortByDuration();
                    SortParams.OrderType = OrderType.Descending;
                    break;
            }

            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        public ICommand ClosePopupPage
        {
            get => new AsyncCommand(async () =>
            {
                this.Disappearing -= UpdateList;
                await App.Current.MainPage.Navigation.PopModalAsync();
            });
        }
    }
}