using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Extensions
{
    /// <summary>
    /// Handles extension methods of <see cref="DiscordBot"/>.
    /// </summary>
    public static class DiscordBotExtensions
    {
        /// <summary>
        /// Fetches the translated string for this bot. Short for <see cref="DiscordBot.Languages.GetString()"/>.
        /// </summary>
        /// <param name="bot"> The bot to get <see cref="LanguageManager"/> from. </param>
        /// <param name="identifier"> The identifier of the translated string. </param>
        /// <returns> The translated string. </returns>
        public static string GetString(this DiscordBot bot, string identifier)
        {
            return bot.Languages.GetString(identifier);
        }
    }
}
