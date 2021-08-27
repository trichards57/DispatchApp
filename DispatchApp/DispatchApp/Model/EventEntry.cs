using System;
using System.Collections.Generic;

namespace DispatchApp.Model
{
    internal class EventEntry
    {
        public Guid Id { get; set; }
        public IList<LogEntry> Log { get; set; } = new List<LogEntry>();
        public string Name { get; set; }
    }
}
