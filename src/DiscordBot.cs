using DCore.Configs;
using DCore.Interfaces;
using DCore;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DCore
{
    /// <summary>
    /// Represents a single Discord bot account.
    /// </summary>
    public class DiscordBot : IBot, IDisposable
    {
        /// <summary>
        /// The <see cref="BotManager"/> that activated this bot.
        /// </summary>
        public BotManager Manager { get; set; }

        /// <summary>
        /// The underlying Discord.NET client.
        /// </summary>
        public DiscordSocketClient Client { get; set; }

        /// <summary>
        /// The <see cref="BotConfig"/> used by the bot.
        /// </summary>
        public BotConfig Config { get; set; }

        /// <summary>
        /// The logger associated with this <see cref="DiscordBot"/>.
        /// </summary>
        public DCoreLogger Logger { get; set; }

        /// <summary>
        /// Manages language data for this bot account (using its language).
        /// </summary>
        public LanguageManager Languages { get; set;}

        /// <summary>
        /// The ID and token information for <see cref="DiscordBot"/>.
        /// </summary>
        public TokenInfo TokenInfo { get; set; }

        /// <summary>
        /// Has the client started and is maintaining the connection?
        /// </summary>
        public bool HasStarted
        {
            get
            {
                if (Client == null)
                    return false;

                return Client.ConnectionState == Discord.ConnectionState.Connected;
            }
        }

        private DiscordSocketConfig _lastConfig;


        /// <summary>
        /// Logs in and starts the <see cref="DiscordBot"/>.
        /// </summary>
        /// <param name="config"> The <see cref="DiscordSocketConfig"/> to use. Null to use default. </param>
        /// <returns> The task that starts up the <see cref="DiscordBot"/>. </returns>
        /// <exception cref="InvalidOperationException"> Thrown when attempting starting on already running bot. </exception>
        public async Task StartAsync(DiscordSocketConfig config = null)
        {
            //If already running, can't start again
            if (Client?.ConnectionState == Discord.ConnectionState.Connected)
                throw new InvalidOperationException("The bot has already started.");

            //If config is null, use the default one
            if (config == null)
                config = new DiscordSocketConfig();
            _lastConfig = config;

            //Initialize the client
            if (Client == null)
                Client = new DiscordSocketClient(config);

            Client.Ready += Ready;

            //Log in and start
            await Client.LoginAsync(Discord.TokenType.Bot, TokenInfo.token);
            await Client.StartAsync();

            //Wait for the connection
            DateTime beganWaiting = DateTime.UtcNow;
            while (Client.ConnectionState != Discord.ConnectionState.Connected)
            {
                await Task.Delay(20);

                //Timeout
                if (beganWaiting + Config.ConnectionTimeout < DateTime.UtcNow)
                    throw new TimeoutException($"Gateway connection for bot {TokenInfo.id} has timed out.");
            }

            //Set the game and status if needed
            if (Manager.DCoreConfig.SetStatusAndGameOnStart)
            {
                _ = Task.Run(() => SetUserStatusAsync());
                _ = Task.Run(() => SetGameAsync());
            }
        }

        /// <summary>
        /// Stops the connection between Discord and the application.
        /// </summary>
        /// <returns> The task that stops the bot. </returns>
        /// <exception cref="InvalidOperationException"> Thrown when attempting to stop a bot that's not running. </exception>
        public async Task StopAsync()
        {
            if (Client == null || Client.ConnectionState == Discord.ConnectionState.Disconnected)
                throw new InvalidOperationException("The bot is not running.");

            await Client.StopAsync();
            await Client.LogoutAsync();
        }

        /// <summary>
        /// Restarts the bot connection.
        /// </summary>
        /// <returns></returns>
        public async Task RestartAsync()
        {
            if (!HasStarted)
                return;

            await StopAsync();
            await StartAsync(_lastConfig);
        }


        /// <summary>
        /// Sets game status using provided values.
        /// </summary>
        /// <param name="activity"> The type of activity. </param>
        /// <param name="game"> The displayed string. </param>
        /// <param name="streamUrl"> The link to the stream, if <see cref="ActivityType"/> is <see cref="ActivityType.Streaming"/>. </param>
        /// <returns> The task that sets the game status. </returns>
        public async Task SetGameAsync(ActivityType activity, string game, string streamUrl = null)
        {
            if (game == null)
                game = "";

            await Client.SetGameAsync(game, streamUrl, activity);
        }

        /// <summary>
        /// Sets the game status using values from config.
        /// </summary>
        /// <returns> The task that sets the game status. </returns>
        public async Task SetGameAsync()
        {
            //Get the values from the config
            ActivityType type = Config.Activity;
            string game = Config.Game;
            string streamUrl = Config.StreamURL;

            //Don't apply the values if set to null
            if (game == null)
                return;

            await Client.SetGameAsync(game, streamUrl, type);
        }

        /// <summary>
        /// Sets the user status using the provided value.
        /// </summary>
        /// <param name="status"> The status to set. </param>
        /// <returns> The task that sets the status. </returns>
        public async Task SetUserStatusAsync(UserStatus status)
        {
            await Client.SetStatusAsync(status);
        }

        /// <summary>
        /// Sets the user status using the value from the config.
        /// </summary>
        /// <returns> The task that sets the status. </returns>
        public async Task SetUserStatusAsync()
        {
            UserStatus status = Config.Status;

            //Don't waste time processing if not needed
            if (status != UserStatus.Online)
                await Client.SetStatusAsync(status);
        }


        /// <summary>
        /// Called when the bot has finished downloading guild data.
        /// </summary>
        /// <returns></returns>
        private Task Ready()
        {
            BotReadyEvent();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Fires when the bot is ready.
        /// </summary>
        public event BotReady BotReadyEvent;
        public delegate void BotReady();


        /// <summary>
        /// Constructs a new DiscordBot
        /// </summary>
        /// <param name="manager"> The BotManager that activated this <see cref="DiscordBot"/>. </param>
        /// <param name="token"> The token information to use. </param>
        /// <param name="overrideConfig"> The config object to override loaded config. </param>
        /// <param name="extensionType"> The <see cref="Type"/> of the config extension. </param>
        public DiscordBot(BotManager manager, TokenInfo token, BotConfig overrideConfig = null, Type extensionType = null)
        {
            Logger = new DCoreLogger(this);
            Languages = new LanguageManager(this, manager.DCoreConfig);
            Manager = manager;
            TokenInfo = token;

            if (overrideConfig != null)
            {
                Config = overrideConfig;
                Config.Save(this);
            }
            else
            {
                //Load the config
                var _loader = new ConfigLoader(Manager.DCoreConfig);
                string path = _loader.GetPathToBotConfig(this);
                Config = _loader.LoadConfig(path, typeof(BotConfig), extensionType) as BotConfig;

                //If config could not be loaded, use the default one
                if (Config == null)
                {
                    Config = manager.ConfigManager.GlobalBotConfig.DefaultBotConfig;
                    Config.Save(this);

                    //Load to ensure it's a deep copy
                    Config = _loader.LoadConfig(path, typeof(BotConfig), extensionType) as BotConfig;
                }
            }

            //Start automatically if required
            if (manager.DCoreConfig.StartBotsOnCreation)
                _ = Task.Run(() => StartAsync());
        }


        private bool disposedValue;
        /// <summary>
        /// Disconnects the bot, removes from list in <see cref="BotManager"/>, and disposes of the resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Client != null)
                        Client.StopAsync().ConfigureAwait(false);
                    Manager._activeBots.Remove(this);
                    Config.Save(this);
                    _lastConfig = null;

                    Client = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
