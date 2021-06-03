using System;

namespace GTAWS_v2.Tools
{
    public class OSTools
    {
        public static string Username => Environment.UserName;
        public static string DesktopPath => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    }
}
