using GTAWS_v2.Enums;
using GTAWS_v2.Extensions;
using GTAWS_v2.Logging;
using GTAWS_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GTAWS_v2.Tools
{
    public class GTATools
    {
        #region Read-Only Variables
        private static readonly string GTA5 = "GTA5";
        private static readonly string EXE = ".exe";
        private static readonly string LogExt = ".log";
        #endregion

        #region Global Variables
        public static string GTA5EEXE => $"{GTA5}{EXE}";

        public static bool IsGTA5Running() => ProcessTools.CheckForProcessByName(GTA5);

        public static bool IsGameLauncherRunning() => LogFileInfo.IsLauncherRunning;

        public static string CurrentGameLauncher => LogFileInfo.GameLauncherName;

        public static string GameLancherEXE => $"{CurrentGameLauncher}{EXE}";

        public static bool IsMultipleLaunchersRunning => LogFileInfo.IsMultipleLaunchersRunning;

        public static List<string> GameLauncherNames => LogFileInfo.GameLauncherNames;

        public static List<string> LogFileNames => GetLaunchers.Select(l => l.LogFileName.RemoveFileExtension()).ToList();

        public static List<Launcher> GetLaunchers => LogFileInfo.AllLaunchers;

        public static string LogFileName => LogFileInfo.LogFileName;

        public static void RunDelay() => Thread.Sleep(2000); // 2 seconds 
        #endregion

        #region PrintProcessInfo
        public static void PrintProcessInfo()
        {
            if (IsGTA5Running())
            {
                LogFileHelper.AddInfoEntry($"{GTA5EEXE} is currently running...", true);
            }
            else
            {
                LogFileHelper.AddWarningEntry($"{GTA5EEXE} is not running...", true);
            }

            if (IsGameLauncherRunning())
            {
                LogFileHelper.AddInfoEntry($"{GameLancherEXE} is currently running...", true);
            }
            else
            {
                LogFileHelper.AddWarningEntry("No Game Launcher detected...", true);
            }
        }
        #endregion

        #region PrintAndExitMessage
        private static void PrintAndExitMessage(string input)
        {
            LogFileHelper.AddErrorEntry($"{input} is not a option...", true);
            LogFileHelper.AddErrorEntry("Incorrect user input...", true);
            LogFileHelper.AddInfoEntry("Please restart GTA5WS.exe...", true);
            LogFileHelper.AddInfoEntry("Press Enter to exit...", true);
            Console.ReadKey();
            OnExit();
        }
        #endregion

        #region PrintFailureMessages
        private static void LoggerFailureMessage() => LogFileHelper.AddErrorEntry("Lucky Wheel Spin Logger Failure...", true);
        private static void LoggerOptionsMessage() => LogFileHelper.AddInfoEntry("Starting Logger Options...", true);
        #endregion
        
        #region StartLogging
        public static void StartLogging(bool nothingDetected = false)
        {
            if (nothingDetected)
            {
                NothingDetectedOptions();
            }
            else
            {
                LogFileHelper.AddInfoEntry($"Logging started to: {LogFileName}...", true);
                PrintProcessInfo();
                LogFileHelper.AddInfoEntry("Starting the Lucky Wheel Spin Logging...", true);
                RunLuckyWheelSpin();
            }
        }
        #endregion

        #region RunLuckyWheelSpin
        private static void RunLuckyWheelSpin()
        {
            LogFileHelper.AddEntry($"{OSTools.Username} spun the Lucky Wheel", LogLevel.Info, true);
            LogFileHelper.AddInfoEntry("Log Lucky Wheel Spin Action Successfully Ran...", true);
            OnExit();
        }
        #endregion

        #region MultipleGameLaunchersDetected
        public static bool MultipleGameLaunchersDetected()
        {
            if (IsMultipleLaunchersRunning)
            {
                LoggerFailureMessage();
                LogFileHelper.AddWarningEntry("Multiple Game Launchers Detected!", true);
                GameLauncherNames.ForEach(n => LogLaunchers(n));
                GameLauncherNames.ForEach(n => LogLaunchersFailure(n));
                LogFileHelper.AddInfoEntry($"Please close one of the launchers and relaunch GTA5WS{EXE}...", true);
                LogFileHelper.AddInfoEntry("Press Enter to exit...", true);
                Console.ReadKey();
                OnExit();
                return true;
            }
            return false;
        }

        private static void LogLaunchers(string name)
        {
            LogFileHelper.AddInfoEntry($"{name}{EXE}", true);
            LogFileHelper.AddInfoEntry($"{name}{EXE} is currently running...");
        }

        private static void LogLaunchersFailure(string name) => LogFileHelper.AddErrorEntry($"Logger Failure Reason: {name}{EXE}is running...");
        #endregion

        #region GTA5NotDetected
        public static void GTA5NotDetected()
        {
            LoggerFailureMessage();
            LogFileHelper.AddErrorEntry($"Logger Failure Reason: {GTA5EEXE} is not running...");
            LogFileHelper.AddInfoEntry($"{GameLancherEXE} is currently running...");
            LoggerOptionsMessage();
            LogFileHelper.AddInfoEntry($"GTA5.exe is not running but {GameLancherEXE} is...", true);
            LoggerOptions();
        }
        #endregion

        #region NothingDetected
        public static void NothingDetected()
        {
            LoggerFailureMessage();
            LogFileHelper.AddErrorEntry($"Logger Failure Reason: {GTA5EEXE} is not running...");
            LogFileHelper.AddErrorEntry($"Logger Failure Reason: Game Launcher Not Found...");
            LoggerOptionsMessage();
            LogFileHelper.AddInfoEntry($"GTA5 is not running and no Game Launcher detected...", true);
            LoggerOptions();
        } 
        #endregion

        #region NothingDetectedOptions
        private static void NothingDetectedOptions()
        {
            LogFileHelper.AddInfoEntry("Choose the file name. Type 1, 2 or 3 and then press Enter:", true);
            PrintGameLauncherNames();
            Console.Write("User Input (1, 2, 3): ");
            string userInput = CheckLogFileType(ReadUserInput());
            LogFileHelper.AddInfoEntry($"User input: {userInput}");
            if (userInput is not "1" and not "2" and not "3")
            {
                PrintAndExitMessage(userInput);
            }
            else
            {
                string logFileName = LogFileNames[Convert.ToInt32(userInput) - 1]; // Need to subtract 1 from the index to get the correct file name
                if (userInput is "1" or "2")
                {
                    LogFileHelper.AddInfoEntry($"Selected: {logFileName}{LogExt}", true);
                    LogFile logger = new(logFileName, LogExt, LogFileInfo.DefaultAppLogFilePath);
                    PrintAndLog(logger, $"{OSTools.Username} spun the Lucky Wheel");
                    OnExit();
                }
                else
                {
                    LogFileHelper.AddInfoEntry($"Selected: {logFileName}", true);
                    RunLuckyWheelSpin();
                }
            }
        }

        public static void PrintGameLauncherNames()
        {
            int count = 1;
            foreach (var l in GetLaunchers)
            {
                if (l.Type == LauncherType.NotFound)
                {
                    LogFileHelper.AddInfoEntry($"{count++}: Default = {l.LogFileName}", true);
                }
                else
                {
                    LogFileHelper.AddInfoEntry($"{count++}: {l.Name}{l.FileExt} = {l.LogFileName}", true);
                }
            }
        }

        public static string CheckLogFileType(string userInput)
        {
            return userInput switch
            {
                "1" => "1",
                "2" => "2",
                "3" => "3",
                _ => userInput,
            };
        }

        public static void PrintAndLog(LogFile logger, string msg)
        {
            Console.WriteLine(msg);
            logger.AddEntry(new LogEntry()
            {
                Data = msg,
                Level = LogLevel.Info,
            });
        }
        #endregion

        #region LoggerOptions
        private static void LoggerOptions()
        {
            LogFileHelper.AddInfoEntry("Would you like to log the wheel spin anyway? Type y or n and press Enter", true);
            Console.Write("User Input (Y/N): ");
            string userInput = ReadUserInput().ToLower();
            if (CorrectInput(userInput))
            {
                if (userInput == "y")
                {
                    LogFileHelper.AddInfoEntry($"User input: {userInput}");
                    StartLogging(true);
                    return;
                }
                if (userInput == "n")
                {
                    LogFileHelper.AddInfoEntry($"User input: {userInput}");
                    OnExit();
                    return;
                }
            }
            else
            {
                PrintAndExitMessage(userInput);
                return;
            }
        }

        private static string ReadUserInput() => Console.ReadLine();

        private static bool CorrectInput(string input) => input is "y" or "n";
        #endregion

        #region OnExit
        public static void OnExit()
        {
            LogFileHelper.AddInfoEntry("Exiting GTA5WS_v2.exe...", true);
            RunDelay();
        }
        #endregion
    }
}
