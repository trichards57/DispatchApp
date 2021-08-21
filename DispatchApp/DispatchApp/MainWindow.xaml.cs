using System;
using DispatchApp.Dialogs;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DispatchApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void WindowActivated(object sender, RoutedEventArgs args)
        {
            var connectDialog = new ConnectDialog();
            connectDialog.XamlRoot = Content.XamlRoot;
            await connectDialog.ShowAsync();
        }
    }
}
