using DCore.Configs;
using DCore.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Extensions
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
        /// Fetches the translated string for this bot. Short for <see cref="DiscordBot.Languages.GetString()"/>.
        /// </summary>
        /// <param name="bot"> The bot to get <see cref="LanguageManager"/> from. </param>
        /// <param name="identifier"> The identifier of the translated string. </param>
        /// <returns> The translated string. </returns>
        public static string GetString(this DiscordBot bot, string identifier)
        {
            return bot.Languages.GetString(identifier);
        }

        /// <summary>
        /// Adds DCore services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services"> The services container to add the services to. </param>
        /// <param name="configAction"> The Action that sets the config. </param>
        /// <param name="extensionType"> The <see cref="Type"/> of config extensions. </param>
        /// <returns> The <see cref="IServiceCollection"/> with DCore services added. </returns>
        public static IServiceCollection AddDCore(this IServiceCollection services, Action<DCoreConfig> configAction = default, Type extensionType = null)
        {
            DCoreConfig config = new DCoreConfig();
            configAction?.Invoke(config);

            //Dirty hack to use ExtensionType
            ConfigManager configManager = new ConfigManager(config, extensionType);

            services
                .AddSingleton(config)
                .AddSingleton(configManager)
                .AddSingleton<BotManager>();

            return services;
        }
    }
}
