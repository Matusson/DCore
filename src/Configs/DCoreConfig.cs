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
    }
}
