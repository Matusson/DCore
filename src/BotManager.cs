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
        /// Loads the accounts into <see cref="BotManager"/>.
        /// </summary>
        /// <param name="accounts"> The accounts to load. </param>
        /// <returns> The amount of accounts that was loaded. </returns>
        public int LoadAccounts(List<TokenInfo> accounts)
        {
            //Ensure only unique information is loaded
            accounts = accounts.Except(_tokens).ToList();

            _tokens.AddRange(accounts);
            return accounts.Count;
        }

        /// <summary>
        /// Creates <paramref name="count"/> new <see cref="DiscordBot"/> instances.
        /// </summary>
        /// <param name="count"> How many instances to create. </param>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when requesting more bots than available, or requesting less bots than 1. </exception>
        /// <returns> List of all bots created. </returns>
        public List<DiscordBot> ActivateBots(int count)
        {
            //Ensure there are enough accounts
            if (count > AvailableBotCount)
                throw new ArgumentOutOfRangeException($"Not enough available bots to fulfill the request. " +
                    $"({AvailableBotCount} available, {count} requested)");

            if (count < 1)
                throw new ArgumentOutOfRangeException("Can't request less bots than 1.");

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

            _activeBots.AddRange(bots);

            return bots;
        }


        /// <summary>
        /// Fetches all active bots.
        /// </summary>
        /// <returns> List of all currently active bots. </returns>
        public List<DiscordBot> GetAllBots()
        {
            return _activeBots;
        }
    }
}
