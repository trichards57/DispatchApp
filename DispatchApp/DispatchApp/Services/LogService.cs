using System;
using System.Threading.Tasks;
using DispatchApp.Model;
using LiteDB;

namespace DispatchApp.Services
{
    internal interface ILogService
    {
        Task AddToLog(string eventName, string controller, string eventDetails);
    }

    internal class LogService : ILogService
    {
        private readonly ILiteDatabase _database;
        private readonly ILiteCollection<EventEntry> _eventCollection;

        public LogService(ILiteDatabase database)
        {
            _database = database;
            _eventCollection = _database.GetCollection<EventEntry>();
        }

        public Task AddToLog(string eventName, string controller, string eventDetails)
        {
            var ev = _eventCollection.Query().Where(e => e.Name == eventName).FirstOrDefault();

            ev?.Log.Add(new LogEntry { ControllerName = controller, EventDetails = eventName, Time = DateTimeOffset.UtcNow });

            return Task.CompletedTask;
        }
    }
}
