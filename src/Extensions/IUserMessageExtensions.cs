using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DCore
{
    /// <summary>
    /// Handles extension methods for IUserMessage from Discord.NET.
    /// </summary>
    public static class IUserMessageExtensions
    {
        /// <summary>
        /// Adds a confirmation emoji to the <paramref name="message"/>.
        /// </summary>
        /// <param name="message"> The message to add the reaction to. </param>
        /// <param name="bot"> The bot to use the emoji from. </param>
        public static void ConfirmWithReact(this IUserMessage message, DiscordBot bot)
        {
            IEmote emote = bot.Config.ConfirmationEmoji;
            _ = Task.Run(() => message.AddReactionAsync(emote));
        }
    }
}
