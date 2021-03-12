using System;
using System.Collections.Generic;
using System.Text;
using DCore.Helpers;

namespace DCore
{
    /// <summary>
    /// Handles extension methods to BotAccountLoader.
    /// </summary>
    public static class BotAccountLoaderExtensions
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
    }
}
