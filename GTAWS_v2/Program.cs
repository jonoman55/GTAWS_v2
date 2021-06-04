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
