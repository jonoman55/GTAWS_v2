using GTAWS_v2.Logging;
using GTAWS_v2.Tools;
using System;

namespace GTAWS_v2
{
    class Program
    {
        static void Main()
        {
            try
            {
                if (!GTATools.IsGTA5Running && GTATools.IsGameLauncherRunning)
                {
                    GTATools.GTA5NotDetected();
                    GTATools.LoggerOptions();
                }
                else if (!GTATools.IsGTA5Running && !GTATools.IsGameLauncherRunning) 
                {
                    GTATools.NothingDetected();
                    GTATools.LoggerOptions();
                }
                else
                {
                    GTATools.StartLogging();
                    return;
                }

                #region Code used for debugging
                //LogFileHelper.AddInfoEntry("GTA5WS_v2 Logging Test...");
                //C:\Users\jonom\Desktop\GTA.lnk
                //LogFileTools.PrintFilesAndFolders(OSTools.DesktopPath);

                //LogFile logger = new("GTA5WS", ".log", @"C:\Users\jonom\Desktop\GTA");
                //logger.CreateLogFileDir();
                //logger.AddEntry(new LogEntry() { Data = "Testing...", Level = Enums.LogLevel.Info });

                //LogFileHelper.AddInfoEntry("Testing Helper Methods..."); 
                #endregion

            }
            catch (Exception ex)
            {
                LogFileHelper.AddErrorEntry(ex.Message);
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}
