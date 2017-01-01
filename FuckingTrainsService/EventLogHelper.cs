using System;
using System.Diagnostics;

namespace FuckingTrainsService
{
    internal class EventLogHelper
    {
        private static readonly EventLog _eventLog = new EventLog();

        static EventLogHelper()
        {
            if (!EventLog.SourceExists("FuckingTrains"))
            {
                EventLog.CreateEventSource(
                    "FuckingTrains", "FuckingTrainsLog");
            }
            _eventLog.Source = "FuckingTrains";
            _eventLog.Log = "FuckingTrainsLog";
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