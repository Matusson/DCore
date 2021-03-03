using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

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
            BotManager manager = new BotManager();
            manager.LoadAccountsFromFile("TestToken.txt");
            DiscordBot bot = manager.ActivateBots(1).FirstOrDefault();

            await bot.StartAsync();

            Assert.IsTrue(bot.Client.ConnectionState == Discord.ConnectionState.Connected);
        }

        [TestMethod()]
        public async Task StartAsync_ExampleConfig()
        {
            BotManager manager = new BotManager();
            manager.LoadAccountsFromFile("TestToken.txt");
            DiscordBot bot = manager.ActivateBots(1).FirstOrDefault();
            DiscordSocketConfig config = new DiscordSocketConfig()
            {
                AlwaysDownloadUsers = false,
                ExclusiveBulkDelete = true,
                MessageCacheSize = 1
            };

            await bot.StartAsync(config);

            Assert.IsTrue(bot.Client.ConnectionState == Discord.ConnectionState.Connected);
        }
    }
}