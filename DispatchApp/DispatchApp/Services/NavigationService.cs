using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using DispatchApp.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace DispatchApp.Services
{
    internal interface INavigationService
    {
        void InitializeFrame(Microsoft.UI.Xaml.Controls.Frame rootFrame);

        void NavigateTo<T>() where T : ObservableObject;

        void NavigateTo<T>(object parameter) where T : ObservableObject;

        void RemoveFromBackStack();
    }

    internal class NavigationService : INavigationService
    {
        private Frame _shellFrame;

        public void InitializeFrame(Frame rootFrame)
        {
            _shellFrame = rootFrame;
            NavigateTo<ConnectViewModel>();
        }

        public void NavigateTo<T>() where T : ObservableObject
        {
            InternalNavigateTo(typeof(T), null);
        }

        public void NavigateTo<T>(object parameter) where T : ObservableObject
        {
            InternalNavigateTo(typeof(T), parameter);
        }

        public void RemoveFromBackStack()
        {
            _ = _shellFrame?.BackStack.Remove(_shellFrame.BackStack.Last());
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("ViewModel", "Page");
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private void InternalNavigateTo(Type viewModelType, object parameter)
        {
            var pageType = GetPageTypeForViewModel(viewModelType);
            _ = _shellFrame?.Navigate(pageType, parameter);
        }
    }
}
