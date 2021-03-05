using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Configs
{
    /// <summary>
    /// Stores various settings related to <see cref="DiscordBot"/>
    /// </summary>
    public class DiscordBotConfig
    {
        /// <summary>
        /// The path where log files will be saved.
        /// </summary>
        public string LogsPath { get; set; } = "\\Logs";

        /// <summary>
        /// Maximum allowed retry count for writing to log files.
        /// </summary>
        public int MaxFileWriteAttempts { get; set; } = 10;

        /// <summary>
        /// Maximum allowed line count per log file.
        /// </summary>
        public int MaxLinesPerLogFile { get; set; } = 2000;
    }
}
