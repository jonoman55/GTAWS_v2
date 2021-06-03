using GTAWS_v2.Enums;
using GTAWS_v2.Logging;
using System;

namespace GTAWS_v2.Tools
{
    public class GTATools
    {
        //read-only variables
        private static readonly string GTA5 = "GTA5";
        private static readonly string EXE = ".EXE";

        public static string GTA5EEXE => $"{GTA5}{EXE}";

        public static bool IsGTA5Running => ProcessTools.CheckForProcessByName(GTA5);

        public static bool IsGameLauncherRunning => LogFileInfo.IsLauncherRunning;

        public static string CurrentGameLauncher => LogFileInfo.GameLauncherName;

        public static void StartLogging()
        {
            LogFileHelper.AddInfoEntry("Starting the Lucky Wheel Spin Logging...");
            RunLuckyWheelSpin();
        }

        public static void NothingDetected() 
        {
            LogFileHelper.AddErrorEntry("Lucky Wheel Spin Logger Failure...");
            LogFileHelper.AddWarningEntry($"{GTA5EEXE} is not running...");
            LogFileHelper.AddWarningEntry($"Logger Failure Reason: {GTA5EEXE} is not running...");
            LogFileHelper.AddWarningEntry("No Game Launcher detected...");
            LogFileHelper.AddWarningEntry($"Logger Failure Reason: Game Launcher Not Found...");
            LogFileHelper.AddInfoEntry("Starting Logger Options...");
            Console.WriteLine($"GTA5 is not running and no Game Launcher detected...");
        }

        public static void GTA5NotDetected()
        {
            LogFileHelper.AddErrorEntry("Lucky Wheel Spin Logger Failure...");
            LogFileHelper.AddWarningEntry($"{GTA5EEXE} is not running...");
            LogFileHelper.AddWarningEntry($"Logger Failure Reason: {GTA5EEXE} is not running...");
            LogFileHelper.AddInfoEntry($"{CurrentGameLauncher} is currently running...");
            LogFileHelper.AddInfoEntry("Starting Logger Options...");
            Console.WriteLine($"GTA5 is not running but the Game Launcher {CurrentGameLauncher} is...");
        }

        public static void RunLuckyWheelSpin()
        {
            LogFileHelper.AddEntry($"{OSTools.Username } spun the Lucky Wheel", LogLevel.Info);
            LogFileHelper.AddInfoEntry("Log Lucky Wheel Spin Action Successfully Ran...");
        }

        public static void LoggerOptions()
        {
            Console.WriteLine("Would you like to log the wheel spin anyway? Type y or n and press Enter");
            Console.Write("User Input (y/n): ");
            string userInput = ReadUserInput();
            if (!CorrectInput(userInput))
            {
                LogFileHelper.AddErrorEntry("Incorrect user input.");
                Console.WriteLine("Input incorrect user input. Please restart GTA5WS.EXE...");
                Console.WriteLine("Press Enter to exit...");
                Console.ReadKey();
                return;
            }
            else
            {
                if (userInput == "y")
                {
                    LogFileHelper.AddInfoEntry($"User input: {userInput}");
                    LogFileHelper.AddInfoEntry("Logging Started...");
                    StartLogging();
                    LogFileHelper.AddInfoEntry("Exiting GTA5WS_v2.EXE...");
                    return;
                }
                if (userInput == "n")
                {
                    LogFileHelper.AddInfoEntry($"User input: {userInput}");
                    LogFileHelper.AddInfoEntry("Exiting GTA5WS_v2.EXE...");
                    return;
                }
            }
        }

        private static string ReadUserInput() => Console.ReadLine();

        private static bool CorrectInput(string input) => input == "y" || input == "n";
    }
}
