using DCore.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCore
{
    /// <summary>
    /// Represents a single Discord bot account.
    /// </summary>
    public class DiscordBot
    {
        /// <summary>
        /// The ID and token information for <see cref="DiscordBot"/>.
        /// </summary>
        public TokenInfo TokenInfo { get; set; }

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
