using GTAWS_v2.Logging;

namespace GTAWS_v2.Interfaces
{
    public interface ILogFile
    {
        public void CreateLogFileDir();
        public void AddEntry(LogEntry entry);
        public void CreateLogFile();
        public bool CheckFileSize(string file);
    }
}
