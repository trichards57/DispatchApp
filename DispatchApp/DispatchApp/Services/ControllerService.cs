using System;
using System.Threading.Tasks;
using DispatchApp.Model;
using LiteDB;

namespace DispatchApp.Services
{
    internal interface IControllerService
    {
        /// <summary>
        /// Logs the provider controller on.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="useLive">if set to <c>true</c> use live the live system, otherwise log in to training mode.</param>
        /// <returns><c>true</c> if log on was successful, otherwise <c>false</c>.</returns>
        Task<bool> LogOn(string controllerName, string eventName, bool useLive);
    }

    internal class ControllerService : IControllerService
    {
        private readonly ILiteDatabase _database;
        private readonly ILiteCollection<EventEntry> _eventCollection;
        private readonly ILogService _logService;
        private string _controllerName;
        private string _eventName;

        public ControllerService(ILiteDatabase database, ILogService logService)
        {
            _database = database;
            _logService = logService;
            _eventCollection = _database.GetCollection<EventEntry>();
        }

        public async Task LogOff()
        {
            if (!string.IsNullOrEmpty(_controllerName))
                await _logService.AddToLog(_eventName, _controllerName, "Logged off.");
        }

        public async Task<bool> LogOn(string controllerName, string eventName, bool useLive)
        {
            _controllerName = controllerName;
            _eventName = eventName;

            var ev = _eventCollection.Query().Where(e => e.Name == eventName).FirstOrDefault();

            if (ev == null)
                _ = _eventCollection.Insert(new EventEntry { Id = Guid.NewGuid(), Name = eventName });

            await _logService.AddToLog(eventName, controllerName, "Logged on.");

            return true;
        }
    }
}
