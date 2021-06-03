using GTAWS_v2.Enums;

namespace GTAWS_v2.Models
{
    public class Launcher
    {
        public LauncherType Type { get; set; }
        public string Name { get; set; }
        public string Ext { get; set; }
        public bool IsRunning { get; set; }

        public override string ToString()
        {
            return $"{Name}.{Ext}";
        }
    }
}
