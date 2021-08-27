using System;

namespace DispatchApp.Model
{
    internal class LogEntry
    {
        public string ControllerName { get; set; }
        public string EventDetails { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
