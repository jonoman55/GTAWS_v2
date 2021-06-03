using System;
using System.Diagnostics;

namespace GTAWS_v2.Extensions
{
    public static class ProcessExtensions
    {
        public static bool IsRunning(this string process)
        {
            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            try
            {
                Process.GetProcessesByName(process.RemoveFileExtension());
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }
    }
}
