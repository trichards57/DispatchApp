using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using CommunityToolkit.Mvvm.DependencyInjection;
using DispatchApp.Services;
using DispatchApp.ViewModels;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

[assembly: InternalsVisibleTo("DispatchApp.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: SupportedOSPlatform("windows10.0.19041")]

namespace DispatchApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window m_window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();

            var dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Dispatcher");
            var dataPath = Path.Combine(dataDirectory, "maindata.db");

            if (!Directory.Exists(dataDirectory))
                _ = Directory.CreateDirectory(dataDirectory);

            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddTransient<ConnectViewModel>()
                    .AddSingleton<INavigationService, NavigationService>()
                    .AddSingleton<IControllerService, ControllerService>()
                    .AddSingleton<IEventService, EventService>()
                    .AddSingleton<ISettingsService, SettingsService>()
                    .AddSingleton<ILogService, LogService>()
                    .AddSingleton<ILiteDatabase>(new LiteDatabase(dataPath))
                    .BuildServiceProvider());
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }
    }
}
