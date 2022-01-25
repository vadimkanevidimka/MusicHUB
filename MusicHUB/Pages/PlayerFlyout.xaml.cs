using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerFlyout : ContentPage
    {
        public ListView ListView;

        public PlayerFlyout()
        {
            InitializeComponent();

            BindingContext = new PlayerFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class PlayerFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<PlayerFlyoutMenuItem> MenuItems { get; set; }
            
            public PlayerFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<PlayerFlyoutMenuItem>(new[]
                {
                    new PlayerFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new PlayerFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new PlayerFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new PlayerFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new PlayerFlyoutMenuItem { Id = 4, Title = "Page 5" },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}