using MusicHUB.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage(Connections connections)
        {
            InitializeComponent();
            Children.Add(new MainPage(connections));
            Children[0].IconImageSource = "library.png";
            Children.Add(new PlayListsPage(connections));
            Children[1].IconImageSource = "playlist2.png";
            Children.Add(new SettingsPage());
            Children[2].IconImageSource = "settingsbutton.png";
        }
    }
}