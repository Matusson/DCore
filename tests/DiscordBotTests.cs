using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using DCore.Helpers;
using DCore.Configs;

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
            DiscordBot bot = new DiscordBot(null, token);

            await bot.StartAsync();

            Assert.IsTrue(bot.Client.ConnectionState == Discord.ConnectionState.Connected);
        }

        [TestMethod()]
        public async Task StartAsync_ThrowsException()
        {
            BotAccountLoader loader = new BotAccountLoader();
            var token = loader.LoadAccountsFromFile("TestToken.txt").FirstOrDefault();
            DiscordBot bot = new DiscordBot(null, token);
            await bot.StartAsync();

            Task func() => bot.StartAsync();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(func);
        }

        [TestMethod()]
        public async Task StartAsync_ExampleConfig()
        {
            BotAccountLoader loader = new BotAccountLoader();
            var token = loader.LoadAccountsFromFile("TestToken.txt").FirstOrDefault();
            DiscordBot bot = new DiscordBot(null, token);
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
            DiscordBot bot = new DiscordBot(null, token);

            await bot.StartAsync();
            await bot.StopAsync();

            var state = bot.Client.ConnectionState;
            Assert.IsTrue((state == Discord.ConnectionState.Disconnected || state == Discord.ConnectionState.Disconnecting)
                && bot.Client.LoginState == Discord.LoginState.LoggedOut);
        }

        [TestMethod()]
        public async Task StopAsync_ThrowsException()
        {
            BotAccountLoader loader = new BotAccountLoader();
            var token = loader.LoadAccountsFromFile("TestToken.txt").FirstOrDefault();
            DiscordBot bot = new DiscordBot(null, token);

            Task func() => bot.StopAsync();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(func);
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public async Task HasStarted(bool start)
        {
            BotAccountLoader loader = new BotAccountLoader();
            var token = loader.LoadAccountsFromFile("TestToken.txt").FirstOrDefault();
            DiscordBot bot = new DiscordBot(null, token);

            if (start)
                await bot.StartAsync();
            bool result = bot.HasStarted;

            Assert.AreEqual(start, result);
        }

        [TestMethod()]
        public void Dispose()
        {
            var manager = new BotManager(new DCoreConfig { UseMultipleBots = true });
            manager.LoadAccountsFromFile("TestToken.txt");
            DiscordBot bot = manager.ActivateBots(1).FirstOrDefault();

            if (manager.AvailableBotCount != 0)
                throw new Exception("Incorrect available bot count.");
            bot.Dispose();

            Assert.IsTrue(manager.AvailableBotCount == 1);
        }
    }
}