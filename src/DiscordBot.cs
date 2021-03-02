﻿using DCore.Structs;
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


        public async Task StartAsync(DiscordSocketConfig config = null)
        {
            //If config is null, use the default one
            if (config == null)
                config = new DiscordSocketConfig();

            //Initialize the client
            if (Client == null)
                Client = new DiscordSocketClient(config);

            //Log in and start
            await Client.LoginAsync(Discord.TokenType.Bot, TokenInfo.token);
            await Client.StartAsync();
        }

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
