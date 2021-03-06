using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Configs
{
    /// <summary>
    /// Stores various settings that should apply globally to all instances of <see cref="DiscordBot"/>.
    /// </summary>
    public class GlobalBotConfig
    {
        /// <summary>
        /// Allow the console to color the outputs?
        /// </summary>
        public bool UseColoredInputInLogs { get; set; } = true;
    }
}
