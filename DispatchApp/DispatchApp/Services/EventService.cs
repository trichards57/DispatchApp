using System.Collections.Generic;
using System.Threading.Tasks;
using DispatchApp.Model;
using LiteDB;

namespace DispatchApp.Services
{
    internal interface IEventService
    {
        Task<IEnumerable<string>> GetEventNames();
    }

    internal class EventService : IEventService
    {
        private readonly ILiteDatabase _database;
        private readonly ILiteCollection<EventEntry> _eventCollection;

        public EventService(ILiteDatabase database)
        {
            _database = database;
            _eventCollection = _database.GetCollection<EventEntry>();
        }

        public Task<IEnumerable<string>> GetEventNames()
        {
            return Task.FromResult<IEnumerable<string>>(_eventCollection.Query().Select(e => e.Name).ToList());
        }
    }
}
