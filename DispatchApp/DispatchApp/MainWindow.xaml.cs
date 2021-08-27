using CommunityToolkit.Mvvm.DependencyInjection;
using DispatchApp.Services;
using Microsoft.UI.Xaml;

namespace DispatchApp
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var navigationService = Ioc.Default.GetRequiredService<INavigationService>();
            navigationService.InitializeFrame(MainFrame);
        }
    }
}
