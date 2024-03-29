﻿using DCore.Configs;
using DCore.Helpers;
using DCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DCore
{
    /// <summary>
    /// Manages <see cref="DiscordBot"/> accounts.
    /// </summary>
    public class BotManager
    {
        /// <summary>
        /// The config manager for this <see cref="BotManager"/> service.
        /// </summary>
        public ConfigManager ConfigManager { get; set; }

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

        internal DCoreConfig DCoreConfig { get; set; }
        internal readonly List<DiscordBot> _activeBots = new List<DiscordBot>();
        private readonly List<TokenInfo> _tokens = new List<TokenInfo>();

        /// <summary>
        /// Loads the accounts into <see cref="BotManager"/>.
        /// </summary>
        /// <param name="accounts"> The accounts to load. </param>
        /// <returns> The amount of accounts that was loaded. </returns>
        /// <exception cref="InvalidOperationException"> Thrown when attempting to load multiple bots with UseMultipleBots sets to false. </exception>
        public int LoadAccounts(List<TokenInfo> accounts)
        {
            //Ensure only unique information is loaded
            accounts = accounts.Except(_tokens).ToList();

            if (accounts.Count + _tokens.Count > 1 && !DCoreConfig.UseMultipleBots)
                throw new InvalidOperationException("You must enable UseMultipleBots in config to allow loading multiple bots.");

            _tokens.AddRange(accounts);
            return accounts.Count;
        }

        /// <summary>
        /// Creates <paramref name="count"/> new <see cref="DiscordBot"/> instances.
        /// </summary>
        /// <param name="count"> How many instances to create. </param>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when requesting more bots than available, or requesting less bots than 1. </exception>
        /// <returns> List of all bots created. </returns>
        public List<DiscordBot> RequestBots(int count, Type configExtensionType = null)
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
                DiscordBot bot = new DiscordBot(this, token, extensionType: configExtensionType);
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

        /// <summary>
        /// Fetches a <see cref="DiscordBot"/> with the specified <paramref name="id"/> assigned.
        /// </summary>
        /// <param name="id"> The ID of the matching bot. </param>
        /// <returns> The <see cref="DiscordBot"/> with this <paramref name="id"/> assigned. </returns>
        /// <exception cref="ArgumentException"> Thrown when a bot with this <paramref name="id"/> doesn't exist or isn't active. </exception>
        public DiscordBot GetBot(ulong id)
        {
            TokenInfo? token = _tokens.Where(x => x.id == id).FirstOrDefault();

            if (!token.HasValue)
                throw new ArgumentException("No tokens with this ID are loaded.");

            return GetBot(token.Value);
        }

        /// <summary>
        /// Fetches a <see cref="DiscordBot"/> with the specified <see cref="TokenInfo"/> assigned.
        /// </summary>
        /// <param name="token"> The <see cref="TokenInfo"/> of the matching bot. </param>
        /// <returns> The <see cref="DiscordBot"/> with this <see cref="TokenInfo"/>. </returns>
        /// <exception cref="ArgumentException"> Thrown when a bot with this <see cref="TokenInfo"/> doesn't exist or isn't active. </exception>
        public DiscordBot GetBot(TokenInfo token)
        {
            DiscordBot bot = _activeBots.Where(x => x.TokenInfo == token).FirstOrDefault();

            if (bot == null)
                throw new ArgumentException("No bots with this ID are active.");

            return bot;
        }


        /// <summary>
        /// Restarts all bots.
        /// </summary>
        public async Task RestartAllAsync()
        {
            foreach(var bot in _activeBots)
            {
                await bot.RestartAsync();
            }
        }

        /// <summary>
        /// Adds DCore-related services to the DI container.
        /// </summary>
        /// <param name="services"> The DI Container. </param>
        /// <exception cref="InvalidOperationException"> Thrown when UseMultipleBots is false and no bots have been loaded. </exception>
        /// <returns> DI Container with DCore services added. </returns>
        public IServiceCollection AddDCoreServices(IServiceCollection services)
        {
            //Add base services
            services
                .AddSingleton(DCoreConfig)
                .AddSingleton(ConfigManager)
                .AddSingleton(this);

            //If using multiple bots, don't add the DiscordSocketClient
            if (DCoreConfig.UseMultipleBots)
                return services;

            if (_activeBots.Count == 0)
                throw new InvalidOperationException("Cannot add DiscordSocketClient to DI Container if no bots are active.");

            DiscordBot bot = _activeBots.First();
            services
                .AddSingleton(bot)
                .AddSingleton(bot.Client)
                .AddSingleton(bot.Logger)
                .AddSingleton(bot.Config)
                .AddSingleton(bot.Languages);

            return services;
        }

        /// <summary>
        /// Constructs a BotManager with specified DCoreConfig and ExtensionType.
        /// </summary>
        /// <param name="configAction"></param>
        /// <param name="extensionType"></param>
        public BotManager(Action<DCoreConfig> configAction = default, Type extensionType = null)
        {
            DCoreConfig = new DCoreConfig();
            configAction?.Invoke(DCoreConfig);

            ConfigManager = new ConfigManager(DCoreConfig, extensionType);
        }

        /// <summary>
        /// Constructs a BotManager with manually specified ConfigManager. Not recommended.
        /// </summary>
        /// <param name="dcoreConfig"> The config to use. </param>
        public BotManager (ConfigManager configService, DCoreConfig dcoreConfig)
        {
            DCoreConfig = dcoreConfig;
            ConfigManager = configService;
        }
    }
}
