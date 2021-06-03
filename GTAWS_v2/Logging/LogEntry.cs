using GTAWS_v2.Enums;
using System;

namespace GTAWS_v2.Logging
{
    public class LogEntry
    {
        public string Data { get; set; }
        public LogLevel Level { get; set; }
        private static DateTime TimeStamp => DateTime.Now;

        public LogEntry() { }
        public LogEntry(string data, LogLevel level)
        {
            Data = data;
            Level = level;
        }

        /// <summary>
        /// The ToString() method needs to be called to format the log Entry.
        /// </summary>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Data))
            {
                return $"{TimeStamp} - {Level} - {Data}"; // format the log entry data
            }
            return null;
        }
    }
}
