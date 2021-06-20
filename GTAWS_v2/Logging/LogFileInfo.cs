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
            new Launcher() { Name = "SteamService", FileExt = "exe", Type = LauncherType.Steam, IsRunning = false },
            new Launcher() { Name = "EpicGamesLauncher", FileExt = "exe", Type = LauncherType.Epic, IsRunning = false }
        };

        // Default Launcher Object
        public static Launcher LauncherNotFound => new() { Name = "Not Found", FileExt = "Not Found", Type = LauncherType.NotFound, IsRunning = false };

        // App Log File Data
        public static  string CurrentLogFileName => IsMultipleLaunchersRunning ? DefaultLogFile : LogFileName;
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
        /// Gets the current Game Launcher running GTA5.exe. <br />
        /// Returns Game Launcher Not Found if multiple launchers or no launchers are detected. <br />
        /// This will also return the log file name as the default -- GTA5WS.log
        /// </summary>
        public static Launcher GameLauncher => !IsMultipleLaunchersRunning && GameLauncherCount != 0 ? GameLaunchersRunning.First() : LauncherNotFound;

        /// <summary>
        /// Get all running Game Launchers
        /// </summary>
        public static List<Launcher> GameLaunchersRunning => GameLaunchers.Where(l => ProcessTools.CheckForProcessByName(l.Name) ? l.IsRunning = true : l.IsRunning = false)
                                                                          .Where(l => l.IsRunning).Select(l => l).ToList();


        /// <summary>
        /// Returns a List or running Game Launcher Names
        /// </summary>
        public static List<string> GameLauncherNames => GameLaunchersRunning.Where(l => l.Name != "Not Found").Select(l => l.Name).ToList();

        /// <summary>
        /// Returns the number of Game Launchers currently running
        /// </summary>
        public static bool IsMultipleLaunchersRunning => GameLauncherCount > 1;

        /// <summary>
        /// Checks if a Launcher is running.
        /// </summary>
        public static bool IsLauncherRunning => GameLauncherCount > 0 && GameLauncherName != "Not Found" && GameLaunchersRunning.First().IsRunning;

        /// <summary>
        /// Returns the number of Game Launchers running
        /// </summary>
        public static int GameLauncherCount => GameLaunchersRunning.Count;

        /// <summary>
        /// Gets the Current Game Launcher name.
        /// </summary>
        public static string GameLauncherName => GameLauncher.Name;
    }
}
