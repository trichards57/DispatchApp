using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DispatchApp.Services;

namespace DispatchApp.ViewModels
{
    internal class ConnectViewModel : ObservableObject
    {
        private readonly IControllerService _controllerService;
        private readonly IEventService _eventService;
        private readonly List<string> _existingEvents = new();
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;
        private bool _connectToLive = true;
        private string _controllerName;
        private bool _errorConnecting = false;
        private string _event;

        public ConnectViewModel(IControllerService controllerService, INavigationService navigationService, IEventService eventService, ISettingsService settingsService)
        {
            _controllerService = controllerService;
            _eventService = eventService;
            _navigationService = navigationService;
            _settingsService = settingsService;
            ConnectCommand = new AsyncRelayCommand(Connect, CanConnect);
        }

        public event EventHandler FailedToConnect;

        public IRelayCommand ConnectCommand { get; }
        public bool ConnectToLive { get => _connectToLive; set => SetProperty(ref _connectToLive, value); }

        public string ControllerName
        {
            get => _controllerName;
            set
            {
                _ = SetProperty(ref _controllerName, value);
                ConnectCommand.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public bool ErrorConnecting { get => _errorConnecting; private set => SetProperty(ref _errorConnecting, value); }

        public string Event
        {
            get => _event;
            set
            {
                _ = SetProperty(ref _event, value);
                UpdateNames();
                ConnectCommand.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public ObservableCollection<string> EventNames { get; set; } = new ObservableCollection<string>();

        public bool IsValid => !string.IsNullOrWhiteSpace(ControllerName) && !string.IsNullOrWhiteSpace(Event);

        public async Task OnNavigateTo()
        {
            var names = await _eventService.GetEventNames();

            _existingEvents.AddRange(names);
            UpdateNames();
            ControllerName = _settingsService.LastControllerName;
        }

        public void UpdateNames()
        {
            EventNames.Clear();

            foreach (var n in _existingEvents.Where(e => e.Contains(Event ?? string.Empty, System.StringComparison.InvariantCultureIgnoreCase)))
                EventNames.Add(n);
        }

        private bool CanConnect()
        {
            return IsValid;
        }

        private async Task Connect()
        {
            ErrorConnecting = false;
            if (await _controllerService.LogOn(ControllerName, Event, ConnectToLive))
            {
                _settingsService.LastControllerName = ControllerName;
                _navigationService.NavigateTo<DispatchViewModel>();
            }
            else
            {
                FailedToConnect?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
