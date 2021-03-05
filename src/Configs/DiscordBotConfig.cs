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
        /// The timeout for connecting to Discord gateway.
        /// </summary>
        public TimeSpan ConnectionTimeout = TimeSpan.FromSeconds(10);
    }
}
