using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Structs
{
    /// <summary>
    /// Stores information related to a <see cref="DiscordBot"/> token.
    /// </summary>
    public struct TokenInfo
    {
        /// <summary>
        /// The ID of the <see cref="DiscordBot"/>.
        /// </summary>
        public ulong id;

        /// <summary>
        /// The token for the <see cref="DiscordBot"/>.
        /// </summary>
        public string token;

        /// <summary>
        /// Constructs a <see cref="TokenInfo"/>.
        /// </summary>
        /// <param name="id"> <inheritdoc cref="id"/> </param>
        /// <param name="token"> <inheritdoc cref="token"/> </param>
        public TokenInfo(ulong id, string token)
        {
            this.id = id;
            this.token = token;
        }
    }
}
