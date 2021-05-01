using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Configs
{
    /// <summary>
    /// Stores various settings related to DCore. These are not saved to a JSON file.
    /// </summary>
    public class DCoreConfig
    {
        /// <summary>
        /// Should creating multiple bots be allowed? Also adds separated log files for each bot and the bot ID to the log output.
        /// </summary>
        public bool UseMultipleBots { get; set; } = false;

        /// <summary>
        /// Should bots be automatically started when they're activated?
        /// </summary>
        public bool StartBotsOnActivation { get; set; } = true;

        /// <summary>
        /// Should the bot set its status and game when started?
        /// </summary>
        public bool SetStatusAndGameOnStart { get; set; } = true;


        /// <summary>
        /// The path where log files will be saved.
        /// </summary>
        public string LogsPath { get; set; } = "Logs\\";

        /// <summary>
        /// The path where config files will be saved.
        /// </summary>
        public string ConfigPath { get; set; } = "Config\\";

        /// <summary>
        /// The path where language files will be read from.
        /// </summary>
        public string LanguagesPath { get; set; } = "Languages\\";


        /// <summary>
        /// The <see cref="DiscordSocketConfig"/> to use by default.
        /// </summary>
        public DiscordSocketConfig DefaultSocketConfig { get; set; } = new DiscordSocketConfig();


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
