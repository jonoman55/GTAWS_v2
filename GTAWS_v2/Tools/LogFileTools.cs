using GTAWS_v2.Extensions;
using System;
using System.IO;

namespace GTAWS_v2.Tools
{
    public class LogFileTools
    {
        public static string GetExeName() => AppDomain.CurrentDomain.FriendlyName.RemoveValue(".vshost").RemoveFileExtension();

        public static string FileSize(string file) => Convert.ToString(Math.Round(new FileInfo(file).Length / 1024f / 1024f)); // Convert the file size to MB

        public static bool CheckFileSize(string file) => Convert.ToInt32(FileSize(file)) > 10;  // File Size limit = 10MB

        public static void TextWriterNew(string path, string data)
        {
            using StreamWriter sw = new(path);
            sw.WriteLine(data);
            sw.WriteLine();
        }

        public static void TextWriterAppend(string path, string data)
        {
            using StreamWriter sw = File.AppendText(path);
            sw.WriteLine(data);
            sw.WriteLine();
        }
    }
}
