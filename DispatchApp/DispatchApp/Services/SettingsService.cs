using Windows.Storage;

namespace DispatchApp.Services
{
    internal interface ISettingsService
    {
        string LastControllerName { get; set; }
    }

    internal class SettingsService : ISettingsService
    {
        private const string LastControllerKey = "LastController";
        private readonly ApplicationDataContainer _settings;

        public SettingsService()
        {
            _settings = ApplicationData.Current.LocalSettings;
        }

        public string LastControllerName
        {
            get => _settings.Values.ContainsKey(LastControllerKey) ? _settings.Values[LastControllerKey].ToString() : string.Empty;
            set => _settings.Values[LastControllerKey] = value;
        }
    }
}
