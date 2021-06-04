using GTAWS_v2.Enums;
using GTAWS_v2.Interfaces;
using GTAWS_v2.Tools;
using System;
using System.IO;

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
}
