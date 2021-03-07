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
        public object Extension { get; set; }


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
        /// IDs of users that can perform high-risk commands for the bot.
        /// </summary>
        public List<ulong> ManagerIDs { get; set; }

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
