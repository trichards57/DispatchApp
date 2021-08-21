using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace DispatchApp.ViewModel
{
    internal class ConnectViewModel : ObservableObject
    {
        private bool _connectToLive;
        private string _controllerName;

        public bool ConnectToLive
        {
            get => _connectToLive;
            set => SetProperty(ref _connectToLive, value);
        }

        public string ControllerName
        {
            get => _controllerName;
            set => SetProperty(ref _controllerName, value);
        }
    }
}
