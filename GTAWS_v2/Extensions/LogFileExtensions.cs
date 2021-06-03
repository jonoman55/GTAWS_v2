using System.IO;

namespace GTAWS_v2.Extensions
{
    public static class LogFileExtensions
    {
        public static string RemoveValue(this string input, string value)
        {
            if (input.ToLower().Contains(value))
            {
                input = input.Replace(value, string.Empty); // replace specified value with an empty string
            }
            return input;
        }

        public static string RemoveFileExtension(this string exeName) => Path.GetFileNameWithoutExtension(exeName);
    }
}
