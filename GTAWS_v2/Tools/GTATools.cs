using GTAWS_v2.Enums;
using GTAWS_v2.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GTAWS_v2.Tools
{
    public class GTATools
    {
        #region Read-Only Variables
        private static readonly string GTA5 = "GTA5";
        private static readonly string EXE = ".exe"; 
        #endregion

        #region Global Variables
        public static string GTA5EEXE => $"{GTA5}{EXE}";

        public static bool IsGTA5Running() => ProcessTools.CheckForProcessByName(GTA5);

        public static bool IsGameLauncherRunning() => LogFileInfo.IsLauncherRunning;

        public static string CurrentGameLauncher => LogFileInfo.GameLauncherName;
        public static string GameLancherEXE => $"{CurrentGameLauncher}{EXE}";

        public static bool IsMultipleLaunchersRunning => LogFileInfo.IsMultipleLaunchersRunning;

        public static List<string> GameLauncherNames => LogFileInfo.GameLauncherNames;

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

        #region OnExit
        public static void OnExit()
        {
            LogFileHelper.AddInfoEntry("Exiting GTA5WS_v2.exe...", true);
            RunDelay();
        } 
        #endregion

        #region StartLogging
        public static void StartLogging()
        {
            PrintProcessInfo();
            LogFileHelper.AddInfoEntry("Starting the Lucky Wheel Spin Logging...", true);
            RunLuckyWheelSpin();
        } 
        #endregion

        #region RunLuckyWheelSpin
        public static void RunLuckyWheelSpin()
        {
            LogFileHelper.AddEntry($"{OSTools.Username } spun the Lucky Wheel", LogLevel.Info, true);
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

        public static void LogLaunchers(string name)
        {
            LogFileHelper.AddInfoEntry($"{name}{EXE}", true);
            LogFileHelper.AddInfoEntry($"{name}{EXE} is currently running...");
        }

        public static void LogLaunchersFailure(string name) => LogFileHelper.AddErrorEntry($"Logger Failure Reason: {name}{EXE}is running..."); 
        #endregion

        #region NothingDetected
        public static void NothingDetected()
        {
            LoggerFailureMessage();
            LogFileHelper.AddErrorEntry($"Logger Failure Reason: {GTA5EEXE} is not running...");
            LogFileHelper.AddErrorEntry($"Logger Failure Reason: Game Launcher Not Found...");
            StartingLoggingMessage();
            LogFileHelper.AddInfoEntry($"GTA5 is not running and no Game Launcher detected...", true);
        } 
        #endregion

        #region GTA5NotDetected
        public static void GTA5NotDetected()
        {
            LoggerFailureMessage();
            LogFileHelper.AddErrorEntry($"Logger Failure Reason: {GTA5EEXE} is not running...");
            LogFileHelper.AddInfoEntry($"{GameLancherEXE} is currently running...");
            StartingLoggingMessage();
            LogFileHelper.AddInfoEntry($"GTA5.exe is not running but {GameLancherEXE} is...", true);
        }
        #endregion

        #region Log Failure Messages
        public static void LoggerFailureMessage() => LogFileHelper.AddErrorEntry("Lucky Wheel Spin Logger Failure...", true);
        public static void StartingLoggingMessage() => LogFileHelper.AddInfoEntry("Starting Logger Options...", true);
        #endregion

        #region LoggerOptions
        public static void LoggerOptions()
        {
            LogFileHelper.AddInfoEntry("Would you like to log the wheel spin anyway? Type y or n and press Enter", true);
            Console.Write("User Input (Y/N): ");
            string userInput = ReadUserInput().ToLower();
            if (CorrectInput(userInput))
            {
                if (userInput == "y")
                {
                    LogFileHelper.AddInfoEntry($"User input: {userInput}");
                    LogFileHelper.AddInfoEntry("Starting Logging...");
                    StartLogging();
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
                LogFileHelper.AddErrorEntry("Incorrect user input.", true);
                LogFileHelper.AddInfoEntry("Please restart GTA5WS.exe...", true);
                LogFileHelper.AddInfoEntry("Press Enter to exit...", true);
                Console.ReadKey();
                OnExit();
                return;
            }
        }

        private static string ReadUserInput() => Console.ReadLine();

        private static bool CorrectInput(string input) => input is "y" or "n"; 
        #endregion
    }
}
