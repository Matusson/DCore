using DCore.Structs;
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
    public class DiscordBot : IDisposable
    {
        /// <summary>
        /// The underlying Discord.NET client.
        /// </summary>
        public DiscordSocketClient Client { get; set; }

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


        private BotManager _manager;
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
            while(Client.ConnectionState != Discord.ConnectionState.Connected)
            {
                await Task.Delay(20);

                //10 second timeout (TODO:Make this configurable)
                if (beganWaiting + TimeSpan.FromSeconds(10) < DateTime.UtcNow)
                    break;
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
            await Client.SetGameAsync(game, streamUrl, activity);
        }

        /// <summary>
        /// Sets the game status using values from config.
        /// </summary>
        /// <returns> The task that sets the game status. </returns>
        public Task SetGameAsync()
        {
            //TODO:Implement this once Configuration system is complete
            throw new NotImplementedException();
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
        /// Constructs a new <see cref="DiscordBot"/>.
        /// </summary>
        /// <param name="token"> The token information to use. </param>
        public DiscordBot (BotManager manager, TokenInfo token)
        {
            _manager = manager;
            TokenInfo = token;
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
                    Client.StopAsync().ConfigureAwait(false);
                    _manager._activeBots.Remove(this);
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
