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
            Children.Add(new PlayListsPage(connections));
        }
    }
}