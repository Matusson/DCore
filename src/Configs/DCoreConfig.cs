using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Configs
{
    /// <summary>
    /// Stores various settings related to DCore.
    /// </summary>
    public class DCoreConfig
    {
        /// <summary>
        /// Should creating multiple bots be allowed? Also adds separated log files for each bot and the bot ID to the log output.
        /// </summary>
        public bool UseMultipleBots { get; set; } = false;

        /// <summary>
        /// Allow the console to color the outputs?
        /// </summary>
        public bool UseColoredInputInLogs { get; set; } = true;

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
