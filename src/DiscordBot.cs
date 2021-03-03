using DCore.Structs;
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
    public class DiscordBot
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
        /// Logs in and starts the <see cref="DiscordBot"/>.
        /// </summary>
        /// <param name="config"> The <see cref="DiscordSocketConfig"/> to use. Null to use default. </param>
        /// <returns> The task that starts up the <see cref="DiscordBot"/>. </returns>
        public async Task StartAsync(DiscordSocketConfig config = null)
        {
            //If config is null, use the default one
            if (config == null)
                config = new DiscordSocketConfig();

            //Initialize the client
            if (Client == null)
                Client = new DiscordSocketClient(config);

            Client.Ready += Ready;

            //Log in and start
            await Client.LoginAsync(Discord.TokenType.Bot, TokenInfo.token);
            await Client.StartAsync();
        }

        /// <summary>
        /// Stops the connection between Discord and the application.
        /// </summary>
        /// <returns> The task that stops the bot. </returns>
        public async Task StopAsync()
        {
            await Client.StopAsync();
            await Client.LogoutAsync();
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
        public DiscordBot (TokenInfo token)
        {
            TokenInfo = token;
        }
    }
}
