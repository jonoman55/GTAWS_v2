using GTAWS_v2.Enums;
using GTAWS_v2.Models;
using GTAWS_v2.Tools;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GTAWS_v2.Logging
{
    public class LogFileInfo
    {
        // Application Data
        public static readonly string AppName = "GTA5WS";
        public static readonly string ExeName = $"{AppName}.exe";
        public static readonly string LogExt = ".log";
        public static readonly string DefaultLogFile = AppName + LogExt;

        // Launchers
        public static List<Launcher> GameLaunchers => new()
        {
            new Launcher() { Name = "SteamService", Ext = "exe", Type = LauncherType.Steam, IsRunning = false },
            new Launcher() { Name = "EpicGamesLauncher", Ext = "exe", Type = LauncherType.Epic, IsRunning = false }
        };

        // Default Launcher Object
        public static Launcher LauncherNotFound => new() { Name = "Not Found", Ext = "Not Found", Type = LauncherType.NotFound, IsRunning = false };

        // App Log File Data
        public static  string CurrentLogFileName => LogFileName;
        public static string DefaultAppLogFilePath => Path.Combine(OSTools.DesktopPath, "GTA");
        public static string DefaultAppLogFileFullPath => Path.Combine(DefaultAppLogFilePath, DefaultLogFile);
        public static string CurrentLogFileFileFullPath => Path.Combine(DefaultAppLogFilePath, CurrentLogFileName);

        /// <summary>
        /// Returns the Full Log File Name including the Log File Extension and File Path. 
        /// </summary>
        public static string FullLogFilePath => GameLauncher.Type != LauncherType.NotFound ? CurrentLogFileFileFullPath : DefaultAppLogFileFullPath;

        /// <summary>
        /// Get The Proper Log File Name from the Launcher Type.
        /// </summary>
        public static string LogFileName
        {
            get
            {
                Launcher launcher = GameLauncher;
                string exeName = launcher.Type switch
                {
                    LauncherType.Steam => "GTA5WSS.log",
                    LauncherType.Epic => "GTA5WSE.log",
                    _ => DefaultLogFile,
                };
                return exeName;
            }
        }

        /// <summary>
        /// Get the current launcher running GTA5.exe. Returns null if no launchers are detected.
        /// </summary>
        public static Launcher GameLauncher
        {
            get
            {
                foreach ((Launcher launcher, string process) in from Launcher launcher in GameLaunchers
                                                                let processName = launcher.Name // Needed for CheckForProcessByName(process)
                                                                select (launcher, processName)) // Extract each Game Launcher to check if it is running.
                {
                    if (!ProcessTools.CheckForProcessByName(process))
                    {
                        continue;
                    }
                    else
                    {
                        launcher.IsRunning = true;
                        return launcher;
                    }
                }
                return LauncherNotFound;
            }
        }

        /// <summary>
        /// Checks if a Launcher is running.
        /// </summary>
        public static bool IsLauncherRunning => GameLauncher.IsRunning;

        /// <summary>
        /// Gets the Current Game Launcher name.
        /// </summary>
        public static string GameLauncherName => GameLauncher.Name;
    }
}
