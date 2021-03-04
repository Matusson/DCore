using DCore.Enums;
using Pastel;
using System;
using System.Collections.Generic;
using System.Text;

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
        internal void WriteToFile(LogType type, string toWrite)
        {
            string finalText = $"{GetPrefix(type)} {toWrite}";
            
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
