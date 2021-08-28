using System;
using CommunityToolkit.Mvvm.DependencyInjection;
using DispatchApp.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Navigation;

namespace DispatchApp.Pages
{
    public sealed partial class ConnectPage : Page
    {
        public ConnectPage()
        {
            InitializeComponent();

            ViewModel.FailedToConnect += FailedToConnect;
        }

        internal ConnectViewModel ViewModel { get; } = Ioc.Default.GetRequiredService<ConnectViewModel>();

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.OnNavigateTo();
        }

        private void FailedToConnect(object sender, EventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(MainGrid);
        }
    }
}
