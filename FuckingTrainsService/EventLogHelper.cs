using System;
using System.Diagnostics;

namespace TrainCommuteCheckService
{
    internal class EventLogHelper
    {
        private static readonly EventLog _eventLog = new EventLog();

        static EventLogHelper()
        {
            if (!EventLog.SourceExists("Trains"))
            {
                EventLog.CreateEventSource(
                    "Trains", "TrainsLog");
            }
            _eventLog.Source = "Trains";
            _eventLog.Log = "TrainsLog";
        }

        internal void WriteEntry(string message)
        {
            _eventLog.WriteEntry(message);
        }

        internal void WriteException(Exception ex)
        {
            _eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
        }
    }
}