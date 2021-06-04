using GTAWS_v2.Enums;
using GTAWS_v2.Interfaces;
using GTAWS_v2.Tools;
using System;
using System.IO;
using System.Linq;

namespace GTAWS_v2.Logging
{
    public class LogFile : ILogFile
    {
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public string FileExt { get; set; }
        public string FolderPath { get; set; }

        /// <summary>
        /// Creates a new instance of a LogFile <br />
        /// File params can be specified but are optional <br />
        /// App EXE name is assumed when not provided
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileExt"></param>                                             
        public LogFile(string fileName = "", string fileExt = "", string filePath = "")
        {
            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileExt) && !string.IsNullOrEmpty(filePath)) // check to see if a file params were specified 
            {
                FileName = fileName;
                FileExt = fileExt;
                FileLocation = Path.Combine(filePath, fileName + fileExt);
                FolderPath = filePath;
            }
            else
            {
                FileExt = LogFileInfo.LogExt;
                FileName = LogFileInfo.LogFileName;
                FileLocation = LogFileInfo.FullLogFilePath;
                FolderPath = LogFileInfo.DefaultAppLogFilePath;
            }

            CreateLogFileDir();
        }

        /// <summary>
        /// Call this method to create the log folder and file.
        /// </summary>
        public void CreateLogFileDir()
        {
            string file = FileLocation;
            string folder = FolderPath;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder); // Create log directory if it doesn't exist
                if (!File.Exists(file))
                {
                    CreateLogFile(); // Create log file if it doesn't exist
                }
            }
        }

        /// <summary>
        /// Call this method to create a log file.
        /// </summary>
        public void CreateLogFile()
        {
            LogEntry entry = new()
            {
                Data = $"{LogFileInfo.AppName}.exe initialized", // remove .log extension from FileName
                Level = LogLevel.Info
            };
            string msg = entry.ToString();
            string file = FileLocation;
            string folder = FolderPath;
            if (Directory.Exists(folder))
            {
                if (!File.Exists(file))
                {
                    LogFileTools.TextWriterNew(file, msg); // Create new log file if it doesn't exist
                }
            }
            else
            {
                CreateLogFileDir(); // Create the log file and folder if it doesn't exist        
            }
        }

        /// <summary>
        /// Adds an Log Entry to the log file.
        /// </summary>
        /// <param name="entry"></param>
        public void AddEntry(LogEntry entry)
        {
            if (entry != null)
            {
                string msg = entry.ToString();
                string file = FileLocation;
                string path = FolderPath;
                if (Directory.Exists(path))
                {
                    if (!File.Exists(file))
                    {
                        // Initial File Creation
                        LogFileTools.TextWriterNew(file, msg);
                    }
                    else
                    {
                        if (!CheckFileSize(file)) // Check the file size to see if it exceeds 10MB
                        {
                            LogFileTools.TextWriterAppend(file, msg); // Create new log file
                        }
                        else
                        {
                            LogFileTools.TextWriterNew(file, msg); // Append to existing file
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(entry)); // entry cannot be null
            }
        }

        /// <summary>
        /// Checks the file the file size. 10MB is max.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool CheckFileSize(string file) => LogFileTools.CheckFileSize(file); // File Size limit = 10MB
    }

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
