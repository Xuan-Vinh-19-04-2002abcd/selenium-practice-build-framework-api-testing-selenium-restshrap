using System.Reflection;

namespace Practice.Core.Extensions;

public static class StringExtensions
{
    public static string GetAbsolutePath(this string filePath)
    {
        string currentDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        filePath = Path.Combine(currentDirectoryPath, filePath);
        if (File.Exists(filePath))
        {
            return filePath;
        }

        return string.Empty;
    }
    
}