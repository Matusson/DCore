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
using Discord;

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
        private DiscordBot CreateTestBot(BotManager manager = null)
        {
            DCoreConfig config = new DCoreConfig();
            if (manager == null)
                manager = new BotManager(new ConfigManager(config), config);

            BotAccountLoader loader = new BotAccountLoader();
            var token = loader.LoadAccountsFromFile("TestToken.txt").FirstOrDefault();
            return new DiscordBot(manager, token);
        }

        [TestMethod()]
        public async Task StartAsync_DefaultConfig()
        {
            DiscordBot bot = CreateTestBot();

            await bot.StartAsync();

            Assert.IsTrue(bot.Client.ConnectionState == Discord.ConnectionState.Connected);
        }

        [TestMethod()]
        public async Task StartAsync_ThrowsException()
        {
            DiscordBot bot = CreateTestBot();
            await bot.StartAsync();

            Task func() => bot.StartAsync();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(func);
        }

        [TestMethod()]
        public async Task StartAsync_ExampleConfig()
        {
            DiscordBot bot = CreateTestBot();
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
            DiscordBot bot = CreateTestBot();

            await bot.StartAsync();
            await bot.StopAsync();

            var state = bot.Client.ConnectionState;
            Assert.IsTrue((state == Discord.ConnectionState.Disconnected || state == Discord.ConnectionState.Disconnecting)
                && bot.Client.LoginState == Discord.LoginState.LoggedOut);
        }

        [TestMethod()]
        public async Task StopAsync_ThrowsException()
        {
            DiscordBot bot = CreateTestBot();

            Task func() => bot.StopAsync();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(func);
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public async Task HasStarted(bool start)
        {
            DiscordBot bot = CreateTestBot();

            if (start)
                await bot.StartAsync();
            bool result = bot.HasStarted;

            Assert.AreEqual(start, result);
        }

        [TestMethod()]
        public async Task SetGameAsyncTest()
        {
            string game = "testing...";
            DiscordBot bot = CreateTestBot();
            bot.Config.Game = game;

            await bot.StartAsync();
            await bot.SetGameAsync();

            Assert.IsTrue(bot.Client.Activity.Type == Discord.ActivityType.Playing &&
                bot.Client.Activity.Name == game);
        }

        [TestMethod()]
        public async Task SetUserStatusTest()
        {
            UserStatus status = UserStatus.DoNotDisturb;
            DiscordBot bot = CreateTestBot();
            bot.Config.Status = status;

            await bot.StartAsync();
            await bot.SetUserStatusAsync();

            Assert.IsTrue(bot.Client.Status == status);
        }

        [TestMethod()]
        public void Dispose()
        {
            DCoreConfig config = new DCoreConfig { UseMultipleBots = true };
            BotManager manager = new BotManager(new ConfigManager(config), config);
            manager.LoadAccountsFromFile("TestToken.txt");
            DiscordBot bot = manager.CreateBots(1).FirstOrDefault();

            if (manager.AvailableBotCount != 0)
                throw new Exception("Incorrect available bot count.");
            bot.Dispose();

            Assert.IsTrue(manager.AvailableBotCount == 1);
        }

        [TestCleanup()]
        public async Task Cleanup()
        {
            await Task.Delay(1000);
        }
    }
}