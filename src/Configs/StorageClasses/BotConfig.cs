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
        /// The timeout for connecting to Discord gateway.
        /// </summary>
        public TimeSpan ConnectionTimeout = TimeSpan.FromSeconds(10);


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
