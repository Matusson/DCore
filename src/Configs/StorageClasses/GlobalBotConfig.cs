﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Configs
{
    /// <summary>
    /// Stores various settings that should apply globally to all instances of <see cref="DiscordBot"/>.
    /// </summary>
    public class GlobalBotConfig : IConfig
    {
        [Newtonsoft.Json.JsonIgnore]
        public object Extension { get; set; }

        /// <summary>
        /// Allow the console to color the outputs?
        /// </summary>
        public bool UseColoredInputInLogs { get; set; } = true;


        /// <summary>
        /// Saves the <see cref="GlobalBotConfig"/> to disc.
        /// </summary>
        /// <param name="manager"> The configuration service. </param>
        public void Save(ConfigManager manager)
        {
            manager.SaveGlobalConfig(this);
        }
    }
}
