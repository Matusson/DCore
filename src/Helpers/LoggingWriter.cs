using DCore.Enums;
using Pastel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Helpers
{
    internal class LoggingWriter
    {
        internal void WriteToConsole(LogType type, string toWrite)
        {

        }

        internal void WriteToFile(LogType type, string toWrite)
        {

        }

        /// <summary>
        /// Gets the prefix for type, optionally colored.
        /// </summary>
        /// <param name="type"> The type of log. </param>
        /// <param name="colored"> Should the prefix be colored? </param>
        /// <returns> The prefix to use for this log type. </returns>
        private string GetPrefix(LogType type, bool colored = false)
        {
            string prefix = "";
            switch (type)
            {
                case LogType.Debug:
                    prefix = "[DEBUG]";
                    if (colored)
                        prefix = prefix.Pastel("#e3d76b");
                    break;

                case LogType.Info:
                    prefix = "[INFO]";
                    break;

                case LogType.Warn:
                    prefix = "[WARN]";
                    if (colored)
                        prefix = prefix.Pastel("#eda334");
                    break;

                case LogType.Error:
                    prefix = "[ERROR]";
                    if (colored)
                        prefix = prefix.Pastel("#eb4b36");
                    break;

                case LogType.CriticalError:
                    prefix = "[DEBUG]";
                    if (colored)
                        prefix = prefix.Pastel("#9e1006");
                    break;

                default:
                    return null;
            }

            return prefix;
        }
    }
}
