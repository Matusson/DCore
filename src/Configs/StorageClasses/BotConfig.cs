using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Configs
{
    /// <summary>
    /// Stores various settings related to an instance of <see cref="DiscordBot"/>
    /// </summary>
    public class BotConfig : IConfig
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
        /// The prefix for the bot commands.
        /// </summary>
        public string Prefix { get; set; } = "!";

        /// <summary>
        /// The emote to use when confirming successful commmands.
        /// </summary>
        public IEmote ConfirmationEmoji { get; set; } = new Emoji("✅");

        /// <summary>
        /// The language to use with this bot.
        /// </summary>
        public string Language { get; set; } = "en-us";

        /// <summary>
        /// Bot's status to use.
        /// </summary>
        public UserStatus Status { get; set; } = UserStatus.Online;

        /// <summary>
        /// Type of activity for the bot status.
        /// </summary>
        public ActivityType Activity { get; set; }

        /// <summary>
        /// The string displayed in bot's status.
        /// </summary>
        public string Game { get; set; }

        /// <summary>
        /// The link to the stream, if <see cref="ActivityType"/> is <see cref="ActivityType.Streaming"/>.
        /// </summary>
        public string StreamURL { get; set; }

        /// <summary>
        /// IDs of users that can perform high-risk commands for the bot.
        /// </summary>
        public List<ulong> ManagerIDs { get; set; } = new List<ulong>();

        /// <summary>
        /// The timeout for connecting to Discord gateway.
        /// </summary>
        public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(10);


        /// <summary>
        /// Saves the <see cref="BotConfig"/> to disc.
        /// </summary>
        /// <param name="bot"> The bot account. </param>
        public void Save(DiscordBot bot)
        {
            var loader = new ConfigLoader(bot.Manager.DCoreConfig);

            string path = loader.GetPathToBotConfig(bot);
            loader.SaveConfig(this, path);
        }
    }
}
