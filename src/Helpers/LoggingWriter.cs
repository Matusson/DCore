using DCore.Enums;
using Pastel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DCore.Helpers
{
    internal class LoggingWriter
    {
        /// <summary>
        /// Writes the specified message to console.
        /// </summary>
        /// <param name="type"> The type of log to write. </param>
        /// <param name="toWrite"> The message to write. </param>
        internal void WriteToConsole(LogType type, string toWrite)
        {
            string finalText = $"{GetPrefix(type, true)} {toWrite}";
            Console.WriteLine(finalText);
        }

        /// <summary>
        /// Writes the specified message to a file.
        /// </summary>
        /// <param name="type"> The type of log to write. </param>
        /// <param name="toWrite"> The message to write. </param>
        internal async Task WriteToFileAsync(LogType type, string path, string toWrite)
        {
            //Ensure the path exists
            string logFolder = Path.GetDirectoryName(path);
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            string finalText = $"{GetPrefix(type)} {toWrite}";

            //TODO:Make some these configurable
            int retryCount = 10;
            int msDelayOnRetry = 100;
            int maxLinesInFile = 2000;

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    List<string> lines = new List<string>(250);

                    //Make sure the file exists, and read it
                    if (File.Exists(path))
                        lines.AddRange(File.ReadAllLines(path));

                    //Trim the line count to avoid taking long time to open the file
                    if (lines.Count > maxLinesInFile)
                    {
                        int toRemove = lines.Count - maxLinesInFile;
                        lines.RemoveRange(0, toRemove);
                    }

                    //Add the line
                    lines.Add(finalText);

                    //Combine the final string
                    string stringToWrite = string.Join("\n", lines);

                    //And write to the file
                    File.WriteAllText(path, stringToWrite);
                    break;
                }
                catch (IOException e)
                {
                    //If the final retry failed, log to console
                    if (i == retryCount - 1)
                        Console.WriteLine(e.ToString());

                    //If the saving failed, retry soon
                    await Task.Delay(msDelayOnRetry);
                }
            }

        }

        /// <summary>
        /// Gets the prefix for type, optionally colored.
        /// </summary>
        /// <param name="type"> The type of log. </param>
        /// <param name="colored"> Should the prefix be colored? </param>
        /// <returns> The prefix to use for this log type. </returns>
        private string GetPrefix(LogType type, bool colored = false)
        {
            string prefix;
            string color;

            switch (type)
            {
                case LogType.Debug:
                    prefix = "[DEBUG]";
                    color = "#e3d76b";
                    break;

                case LogType.Info:
                    prefix = "[INFO] ";
                    color = "#ffffff";
                    break;

                case LogType.Warn:
                    prefix = "[WARN] ";
                    color = "#eda334";
                    break;

                case LogType.Error:
                    prefix = "[ERROR]";
                    color = "#db1818";
                    break;

                case LogType.CriticalError:
                    prefix = "[CRITICAL]";
                    color = "#ed0000";
                    break;

                default:
                    return null;
            }

            prefix += $" {GetTimeString()}";

            //TODO:Add config variable for colored strings
            if (colored)
                prefix = prefix.Pastel(color);

            return prefix + " |";
        }

        /// <summary>
        /// Returns the current time formatted as a string.
        /// </summary>
        /// <returns></returns>
        private string GetTimeString()
        {
            return $"{DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToLongTimeString()}";
        }
    }
}
