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


        public static bool operator ==(TokenInfo c1, TokenInfo c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(TokenInfo c1, TokenInfo c2)
        {
            return !c1.Equals(c2);
        }


        public override bool Equals(object obj)
        {
            return obj is TokenInfo info &&
                   id == info.id &&
                   token == info.token;
        }

        public override int GetHashCode()
        {
            int hashCode = 73361426;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(token);
            return hashCode;
        }
    }
}
