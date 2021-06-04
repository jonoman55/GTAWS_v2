using GTAWS_v2.Enums;
using System.Linq;

namespace GTAWS_v2.Logging
{
    public class LogFileHelper
    {
        /// <summary>
        /// Used for adding an entry to the log file without having to create a new LogFile object.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="level"></param>
        public static void AddEntry(string data, LogLevel level)
        {
            AddEntries(new LogEntry() { Data = data, Level = level });
        }

        /// <summary>
        /// Used for logging multiple Log Entries to the log file.
        /// </summary>
        public static void AddEntries(params LogEntry[] entries)
        {
            foreach ((LogEntry e, string msg) in from e in entries
                                                 let data = e.ToString()
                                                 select (e, data)) // Extract The Log Entry and message from the Array
            {
                if (e == null || string.IsNullOrEmpty(msg)) // Check if the data is valid
                {
                    continue;
                }
                else
                {
                    new LogFile().AddEntry(e); // Add the Log Entry to the log file.
                }
            }
        }

        public static void AddWarningEntry(string msg)
        {
            AddEntry(msg, LogLevel.Warning);
        }

        public static void AddErrorEntry(string msg)
        {
            AddEntry(msg, LogLevel.Error);
        }

        public static void AddInfoEntry(string msg)
        {
            AddEntry(msg, LogLevel.Info);
        }

        public static void AddDebugEntry(string msg)
        {
            AddEntry(msg, LogLevel.Debug);
        }
    }
}
