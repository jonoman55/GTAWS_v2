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
                // Logging Logic
                // If Multiple Game Laucher's are running -- Not allowed -- Only one instance at a time.
                if (GTATools.MultipleGameLaunchersDetected())
                {
                    return;
                } 
                //  If GTA5.exe is not running but a Game Launcher is running.
                else if (!GTATools.IsGTA5Running() && GTATools.IsGameLauncherRunning())
                {
                    GTATools.GTA5NotDetected();
                    GTATools.LoggerOptions();
                } 
                // If GTA5.exe is not running and if no Game Launchers are running.
                else if (!GTATools.IsGTA5Running() && !GTATools.IsGameLauncherRunning())
                {
                    GTATools.NothingDetected();
                    GTATools.LoggerOptions();
                } 
                // If GTA5.exe is running and a single Game Launcher is running.
                else
                {
                    GTATools.StartLogging();
                    return;
                }
            }
            catch (Exception ex)
            {
                LogFileHelper.AddErrorEntry(ex.Message, true);
                return;
            }
        }
    }
}
