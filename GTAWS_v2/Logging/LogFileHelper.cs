using GTAWS_v2.Enums;
using System;
using System.Linq;

namespace GTAWS_v2.Logging
{
    public class LogFileHelper
    {
        /// <summary>
        /// Used for adding an entry to the log file without having to create a new LogFile object. <br />
        /// Pass true to print to Write message to the console window.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="level"></param>
        /// <param name="print"></param>
        public static void AddEntry(string data, LogLevel level, bool print = false)
        {
            AddEntries(print, new LogEntry() { Data = data, Level = level });
        }

        /// <summary>
        /// Used for logging multiple Log Entries to the log file.
        /// </summary>
        public static void AddEntries(bool print, params LogEntry[] entries)
        {
            foreach ((LogEntry e, string msg) in from e in entries
                                                 let data = e.ToString()
                                                 select (e, data)) // Extract The Log Entry and message from the Array
            {
                if (e != null && !string.IsNullOrEmpty(msg)) // Check if the data is valid
                {
                    new LogFile().AddEntry(e); // Add the Log Entry to the log file.
                    if (print) 
                    {
                        Console.WriteLine(e.Data); // Print message to console window if true
                    }

                }
                else
                {
                    continue;
                }
            }
        }

        public static void AddWarningEntry(string msg, bool print = false)
        {
            AddEntry(msg, LogLevel.Warning, print);
        }

        public static void AddErrorEntry(string msg, bool print = false)
        {
            AddEntry(msg, LogLevel.Error, print);
        }

        public static void AddInfoEntry(string msg, bool print = false)
        {
            AddEntry(msg, LogLevel.Info, print);
        }

        public static void AddDebugEntry(string msg, bool print = false)
        {
            AddEntry(msg, LogLevel.Debug, print);
        }
    }
}
