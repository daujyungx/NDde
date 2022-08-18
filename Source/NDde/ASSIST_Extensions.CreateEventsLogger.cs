// ASSIST_Extensions.CreateEventsLogger
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ASSIST_Extensions
{
    /// <summary>
    /// Creates an event logger using the specified
    /// </summary>
    public class CreateEventsLogger
    {
        private static EventLog EventLog { get; set; } = new EventLog();

        /// <summary>
        /// Creates the event logger of Log name and Source datasource if the Log doesn't exist. Otherwise uses the exist event log with the same name.
        /// If no log is specified, it uses the default Application Log.
        /// </summary>
        /// <param name="source">The source. This goes into the DataSource column</param>
        /// <param name="log">The log. This is the name of the log the events are written to.</param>
        /// <returns></returns>
        public static EventLog CreaterEventLogger(string source, string log = "Application")
        {
            try
            {
                EventLog = new EventLog
                {
                    Source = source,
                    Log = log
                };
                if (!EventLog.SourceExists(EventLog.Source))
                {
                    EventLog.CreateEventSource(EventLog.Source, EventLog.Log);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} - {ex.StackTrace}");
            }
            return EventLog;
        }

        /// <summary>
        /// Accepts an Event Log Name and log level. Returns all log entries for that Event Log and level
        /// The default log (if none is specified) is the Application log and default log level is FailueAudit
        /// </summary>
        /// <param name="eventLogName">Name of the event log.</param>
        /// <param name="logType">Type of the log.</param>
        /// <param name="onlyEventsEqualTo"></param>
        /// <returns>List<EvengLogEntry /> EventList</returns>
        public static List<EventLogEntry> GetEvents<T>(string eventLogName = "Application", EventLogEntryType logType = EventLogEntryType.FailureAudit, bool onlyEventsEqualTo = false)
        {
            int copyToArryCnt = 0;
            EventLog = new EventLog
            {
                Log = eventLogName
            };
            EventLogEntryCollection eventLogEntryCollection = EventLog.Entries;
            EventLogEntry[] myEventLogEntryArray = new EventLogEntry[eventLogEntryCollection.Count];
            try
            {
                foreach (EventLogEntry entry in eventLogEntryCollection)
                {
                    int elNumber = (int)entry.EntryType;
                    switch (onlyEventsEqualTo)
                    {
                        case false:
                            if (elNumber <= (int)logType)
                            {
                                myEventLogEntryArray[copyToArryCnt] = entry;
                                copyToArryCnt++;
                            }
                            break;

                        case true:
                            if (elNumber == (int)logType)
                            {
                                myEventLogEntryArray[copyToArryCnt] = entry;
                                copyToArryCnt++;
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return myEventLogEntryArray.Where((EventLogEntry rec) => rec != null).ToList();
        }
    }
}
