using System;
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
        /// The list of all user IDs that can manage all bots.
        /// </summary>
        public List<ulong> ManagerIDs { get; set; } = new List<ulong>();

        /// <summary>
        /// The default config used by all newly activated bots.
        /// </summary>
        public BotConfig DefaultBotConfig { get; set; } = new BotConfig();

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
