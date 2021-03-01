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
        private List<TokenInfo> _tokens = new List<TokenInfo>();

        /// <summary>
        /// Loads ID and token information from a .txt file.
        /// </summary>
        /// <param name="pathToFile"> The path to the .txt file containing data. </param>
        /// <exception cref="FileNotFoundException"> Thrown when <paramref name="pathToFile"/> does not exist. </exception>
        /// <exception cref="ArgumentException"> Thrown when the file is empty or incorrectly formatted. </exception>
        /// <returns> The amount of accounts that were loaded. </returns>
        public int LoadAccounts(string pathToFile)
        {
            if (!File.Exists(pathToFile))
                throw new FileNotFoundException("The specified file does not exist.");

            //Open the file
            string[] lines = File.ReadAllLines(pathToFile);

            //If the file was empty
            if (lines.Length == 0)
                throw new ArgumentException("The specified file was empty.");

            int addedTokens = 0;
            string incorrectlyFormattedException = "File was incorrectly formatted. Make sure that each line starts with the ID " +
                        "followed by a token, separated by a space.";

            //In every line, there should be an ulong ID and a string TOKEN separated by a space
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                string[] perLineInformation = line.Split(' ', ',');

                //Throw an exception if the data is incorrectly formatted
                if (perLineInformation.Length != 2)
                    throw new ArgumentException(incorrectlyFormattedException);

                //The first value should be an ULONG
                if (!ulong.TryParse(perLineInformation[0], out ulong id))
                    throw new ArgumentException(incorrectlyFormattedException);

                //The second value should be a string
                string token = perLineInformation[1];


                TokenInfo tokenInfo = new TokenInfo(id, token);

                //Only add the token if it doesn't already exist
                if (!_tokens.Any(x => x.id == tokenInfo.id))
                {
                    _tokens.Add(tokenInfo);
                    addedTokens++;
                }
            }

            return addedTokens;
        }


    }
}
