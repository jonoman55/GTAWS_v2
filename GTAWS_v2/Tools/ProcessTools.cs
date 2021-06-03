using GTAWS_v2.Extensions;
using System.Diagnostics;

namespace GTAWS_v2.Tools
{
    public class ProcessTools
    {
        public static int GetProcessId(string procName)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName == procName)
                {
                    return process.Id;
                }
            }
            return 0;
        }

        public static bool CheckForProcessByName(string procName)
        {
            foreach (Process p in Process.GetProcessesByName(procName.RemoveFileExtension()))
            {
                try
                {
                    if (procName.Contains(p.ProcessName))
                    {
                        return true;
                    }
                }
                catch
                {
                    continue;
                }
            }
            return false;
        }

        public static bool CheckForCurrentProcess() => Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1;
    }
}
