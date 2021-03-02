using DCore.Helpers;
using DCore.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DCore
{
    /// <summary>
    /// Manages <see cref="DiscordBot"/> accounts.
    /// </summary>
    public class BotManager
    {
        /// <summary>
        /// The total amount of bot accounts loaded.
        /// </summary>
        public int TotalBotCount
        {
            get
            {
                return _tokens.Count();
            }
        }

        /// <summary>
        /// The amount of bot accounts that aren't in use.
        /// </summary>
        public int AvailableBotCount
        {
            get
            {
                return TotalBotCount - _activeBots.Count();
            }
        }

        private readonly List<DiscordBot> _activeBots = new List<DiscordBot>();
        private readonly List<TokenInfo> _tokens = new List<TokenInfo>();

        /// <summary>
        /// Loads ID and token information from a .txt file.
        /// </summary>
        /// <param name="pathToFile"> The path to the .txt file containing data. </param>
        /// <exception cref="FileNotFoundException"> Thrown when <paramref name="pathToFile"/> does not exist. </exception>
        /// <exception cref="ArgumentException"> Thrown when the file is empty or incorrectly formatted. </exception>
        /// <returns> The amount of accounts that were loaded. </returns>
        public int LoadAccounts(string pathToFile)
        {
            BotAccountLoader loader = new BotAccountLoader();
            var tokens = loader.LoadAccountsFromFile(pathToFile);

            //Ensure only unique information is loaded
            tokens = tokens.Except(_tokens).ToList();

            _tokens.AddRange(tokens);
            return tokens.Count;
        }


        /// <summary>
        /// Creates <paramref name="count"/> new <see cref="DiscordBot"/> instances.
        /// </summary>
        /// <param name="count"> How many instances to create. </param>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when requesting more bots than available. </exception>
        /// <returns> List of all bots created. </returns>
        public List<DiscordBot> CreateBots(int count)
        {
            //Ensure there are enough accounts
            if (count > AvailableBotCount)
                throw new ArgumentOutOfRangeException($"Not enough available bots to fulfill the request. " +
                    $"({AvailableBotCount} available, {count} requested)");

            //Create the requested amount of bots
            //Find x unused tokens
            var unusedTokens = _tokens.Where(x => !_activeBots.Any(y => y.TokenInfo == x)).Take(count).ToList();

            //Then use each token to create a Discord bot
            List<DiscordBot> bots = new List<DiscordBot>();
            foreach(TokenInfo token in unusedTokens)
            {
                DiscordBot bot = new DiscordBot(token);
                bots.Add(bot);
            }

            return bots;
        }
    }
}
