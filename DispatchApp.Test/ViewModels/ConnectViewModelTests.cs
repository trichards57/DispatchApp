using System.Threading.Tasks;
using AutoFixture;
using DispatchApp.Services;
using DispatchApp.ViewModels;
using FluentAssertions;
using Moq;
using Xunit;

namespace DispatchApp.Test.ViewModels
{
    public class ConnectViewModelTests
    {
        [Fact]
        public void AlertsOnFailedConnect()
        {
            var fixture = new Fixture();
            var eventNames = fixture.CreateMany<string>();
            var controllerName = fixture.Create<string>();
            var eventName = fixture.Create<string>();

            var eventService = new Mock<IEventService>();
            var navigationService = new Mock<INavigationService>();
            var controllerService = new Mock<IControllerService>();
            controllerService.Setup(s => s.LogOn(controllerName, eventName, true)).ReturnsAsync(false);
            var settingsService = new Mock<ISettingsService>();

            var viewModel = new ConnectViewModel(controllerService.Object, navigationService.Object, eventService.Object, settingsService.Object);

            using var monitoredViewModel = viewModel.Monitor();

            viewModel.ConnectToLive = true;
            viewModel.ControllerName = controllerName;
            viewModel.Event = eventName;

            viewModel.ConnectCommand.Execute(null);

            navigationService.Verify(s => s.NavigateTo<DispatchViewModel>(), Times.Never);
            monitoredViewModel.Should().Raise(nameof(viewModel.FailedToConnect));
        }

        [Fact]
        public void AllowsConnectionWhenControllerAndEventGiven()
        {
            var fixture = new Fixture();
            var controllerName = fixture.Create<string>();
            var eventName = fixture.Create<string>();

            var eventService = new Mock<IEventService>();
            var navigationService = new Mock<INavigationService>();
            var controllerService = new Mock<IControllerService>();
            var settingsService = new Mock<ISettingsService>();

            var viewModel = new ConnectViewModel(controllerService.Object, navigationService.Object, eventService.Object, settingsService.Object);

            viewModel.ControllerName = string.Empty;
            viewModel.Event = string.Empty;
            viewModel.IsValid.Should().BeFalse();
            viewModel.ConnectCommand.CanExecute(null).Should().BeFalse();

            viewModel.ControllerName = controllerName;
            viewModel.Event = string.Empty;
            viewModel.IsValid.Should().BeFalse();
            viewModel.ConnectCommand.CanExecute(null).Should().BeFalse();

            viewModel.ControllerName = string.Empty;
            viewModel.Event = eventName;
            viewModel.IsValid.Should().BeFalse();
            viewModel.ConnectCommand.CanExecute(null).Should().BeFalse();

            viewModel.ControllerName = controllerName;
            viewModel.Event = eventName;
            viewModel.IsValid.Should().BeTrue();
            viewModel.ConnectCommand.CanExecute(null).Should().BeTrue();
        }

        [Fact]
        public async Task FiltersEventsOnInput()
        {
            var fixture = new Fixture();
            var eventNames = new string[]
            {
                "A Event",
                "B Event",
                "B2 Event",
                "Event b3",
                "Event c8"
            };
            var expectedEventNames = new string[]
            {
                "B Event",
                "B2 Event",
                "Event b3",
            };

            var eventService = new Mock<IEventService>();
            eventService.Setup(s => s.GetEventNames()).ReturnsAsync(eventNames);
            var navigationService = new Mock<INavigationService>();
            var controllerService = new Mock<IControllerService>();
            var settingsService = new Mock<ISettingsService>();

            var viewModel = new ConnectViewModel(controllerService.Object, navigationService.Object, eventService.Object, settingsService.Object);

            await viewModel.OnNavigateTo();

            viewModel.Event = "b";

            viewModel.EventNames.Should().BeEquivalentTo(expectedEventNames);

            viewModel.Event = "b3 e";

            viewModel.EventNames.Should().BeEmpty();
        }

        [Fact]
        public async Task LoadsLastControllerFromSettingsOnNavigateTo()
        {
            var fixture = new Fixture();
            var controllerName = fixture.Create<string>();

            var eventService = new Mock<IEventService>();
            var navigationService = new Mock<INavigationService>();
            var controllerService = new Mock<IControllerService>();
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(s => s.LastControllerName).Returns(controllerName);

            var viewModel = new ConnectViewModel(controllerService.Object, navigationService.Object, eventService.Object, settingsService.Object);

            await viewModel.OnNavigateTo();

            viewModel.ControllerName.Should().Be(controllerName);
        }

        [Fact]
        public async Task LoadsPossibleEventsOnNavigateTo()
        {
            var fixture = new Fixture();
            var eventNames = fixture.CreateMany<string>();

            var eventService = new Mock<IEventService>();
            eventService.Setup(s => s.GetEventNames()).ReturnsAsync(eventNames);
            var navigationService = new Mock<INavigationService>();
            var controllerService = new Mock<IControllerService>();
            var settingsService = new Mock<ISettingsService>();

            var viewModel = new ConnectViewModel(controllerService.Object, navigationService.Object, eventService.Object, settingsService.Object);

            await viewModel.OnNavigateTo();

            viewModel.EventNames.Should().BeEquivalentTo(eventNames);
        }

        [Fact]
        public void NavigatesToMainViewOnSuccessfulConnect()
        {
            var fixture = new Fixture();
            var eventNames = fixture.CreateMany<string>();
            var controllerName = fixture.Create<string>();
            var eventName = fixture.Create<string>();

            var eventService = new Mock<IEventService>();
            var navigationService = new Mock<INavigationService>();
            var controllerService = new Mock<IControllerService>();
            controllerService.Setup(s => s.LogOn(controllerName, eventName, true)).ReturnsAsync(true);
            var settingsService = new Mock<ISettingsService>();

            var viewModel = new ConnectViewModel(controllerService.Object, navigationService.Object, eventService.Object, settingsService.Object);

            viewModel.ConnectToLive = true;
            viewModel.ControllerName = controllerName;
            viewModel.Event = eventName;

            viewModel.ConnectCommand.Execute(null);

            navigationService.Verify(s => s.NavigateTo<DispatchViewModel>());
        }
    }
}
