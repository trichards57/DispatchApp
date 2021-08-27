using CommunityToolkit.Mvvm.DependencyInjection;
using DispatchApp.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace DispatchApp.Pages
{
    public sealed partial class ConnectPage : Page
    {
        public ConnectPage()
        {
            InitializeComponent();
        }

        internal ConnectViewModel ViewModel { get; } = Ioc.Default.GetRequiredService<ConnectViewModel>();

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.OnNavigateTo();
        }
    }
}
