using GTAWS_v2.Enums;

namespace GTAWS_v2.Models
{
    public class Launcher
    {
        public LauncherType Type { get; set; }
        public string Name { get; set; }
        public string FileExt { get; set; }
        public bool IsRunning { get; set; }

        /// <summary>
        /// Appends .exe to the Launchers name <br />
        /// For example: SteamService.exe and EpicGamesLauncher.exe
        /// </summary>
        public override string ToString()
        {
            return $"{Name}.{FileExt}";
        }
    }
}
