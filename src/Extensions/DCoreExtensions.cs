﻿using DCore.Configs;
using DCore.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DCore
{
    /// <summary>
    /// Handles extension methods of DCore modules.
    /// </summary>
    public static class DCoreExtensions
    {
        /// <summary>
        /// <inheritdoc cref="BotAccountLoader.LoadAccountsFromFile(string)"/>
        /// </summary>
        /// <param name="manager"> The <see cref="BotManager"/> to load to. </param>
        /// <param name="pathToFile"> The path to the .txt file containing data. </param>
        /// <exception cref="FileNotFoundException"> Thrown when <paramref name="pathToFile"/> does not exist. </exception>
        /// <exception cref="ArgumentException"> Thrown when the file is empty or incorrectly formatted. </exception>
        /// <returns> The amount of accounts that were loaded. </returns>
        public static int LoadAccountsFromFile(this BotManager manager, string pathToFile)
        {
            //Load the tokens
            BotAccountLoader loader = new BotAccountLoader();
            var tokens = loader.LoadAccountsFromFile(pathToFile);

            return manager.LoadAccounts(tokens);
        }

        /// <summary>
        /// Loads a single account to the <see cref="BotManager"/>
        /// </summary>
        /// <param name="manager"> The <see cref="BotManager"/> to load to. </param>
        /// <param name="botId"> The ID of the bot. </param>
        /// <returns> The amount of accounts that were loaded. </returns>
        public static int LoadAccount(this BotManager manager, ulong botId, string botToken)
        {
            //Create the token object
            TokenInfo info = new TokenInfo(botId, botToken);

            return manager.LoadAccounts(new List<TokenInfo> { info });
        }

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
