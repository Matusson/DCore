using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using DCore.Helpers;

namespace DCore.Tests
{
    /* WARNING!
     * This test class requires an ID and token to be provided in the TestToken.txt file.
     * Otherwise the bot will not establish a connection and the tests will fail.
     */

    [TestClass()]
    [DeploymentItem("TestToken.txt")]
    public class DiscordBotTests
    {
        [TestMethod()]
        public async Task StartAsync_DefaultConfig()
        {
            BotAccountLoader loader = new BotAccountLoader();
            var token = loader.LoadAccountsFromFile("TestToken.txt").FirstOrDefault();
            DiscordBot bot = new DiscordBot(token);

            await bot.StartAsync();

            Assert.IsTrue(bot.Client.ConnectionState == Discord.ConnectionState.Connected);
        }

        [TestMethod()]
        public async Task StartAsync_ExampleConfig()
        {
            BotAccountLoader loader = new BotAccountLoader();
            var token = loader.LoadAccountsFromFile("TestToken.txt").FirstOrDefault();
            DiscordBot bot = new DiscordBot(token);
            DiscordSocketConfig config = new DiscordSocketConfig()
            {
                AlwaysDownloadUsers = false,
                ExclusiveBulkDelete = true,
                MessageCacheSize = 1
            };

            await bot.StartAsync(config);

            Assert.IsTrue(bot.Client.ConnectionState == Discord.ConnectionState.Connected);
        }

        [TestMethod()]
        public async Task StopAsync()
        {
            BotAccountLoader loader = new BotAccountLoader();
            var token = loader.LoadAccountsFromFile("TestToken.txt").FirstOrDefault();
            DiscordBot bot = new DiscordBot(token);

            await bot.StartAsync();
            await bot.StopAsync();

            var state = bot.Client.ConnectionState;
            Assert.IsTrue((state == Discord.ConnectionState.Disconnected || state == Discord.ConnectionState.Disconnecting)
                && bot.Client.LoginState == Discord.LoginState.LoggedOut);
        }
    }
}