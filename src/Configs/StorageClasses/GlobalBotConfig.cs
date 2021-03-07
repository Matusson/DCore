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
        public object Extension
        {
            get
            {
                if (_extension == null)
                {
                    throw new InvalidOperationException("You attempted to fetch the Extension object, but it was not set. " +
                        "To use the Extension object, initialize the bot with the its type.");
                }
                return _extension;
            }
            set
            {
                _extension = value;
            }
        }
        private object _extension;

        public bool IsExtensionNull
        {
            get
            {
                return _extension == null;
            }
        }

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
