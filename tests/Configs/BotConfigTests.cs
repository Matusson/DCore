using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore.Configs;
using System;
using System.Collections.Generic;
using System.Text;
using DCore.Tests;
using System.Linq;
using System.IO;

namespace DCore.Configs.Tests
{
    [TestClass()]
    public class BotConfigTests
    {
        readonly DCoreConfig _config = new DCoreConfig();

        [TestMethod()]
        public void CreateNew()
        {
            string pathToFile = Path.Combine(_config.ConfigPath, "12345.json");
            if (File.Exists(pathToFile))
                File.Delete(pathToFile);

            var manager = BotManagerTests.CreateBotManager(1);
            var bot = manager.ActivateBots(1, typeof(GlobalBotConfig)).FirstOrDefault();

            BotConfig config = bot.Config;

            Assert.IsTrue(config != null && File.Exists(pathToFile));
        }

        [TestMethod()]
        public void CreateNew_Extension()
        {
            string pathToFile = Path.Combine(_config.ConfigPath, "12345-e.json");
            if (File.Exists(pathToFile))
                File.Delete(pathToFile);

            var manager = BotManagerTests.CreateBotManager(1);
            var bot = manager.ActivateBots(1, typeof(GlobalBotConfig)).FirstOrDefault();

            BotConfig config = bot.Config;

            Assert.IsTrue(config != null && File.Exists(pathToFile));
        }

        [TestMethod()]
        public void LoadExisting()
        {
            string pathToFile = Path.Combine(_config.ConfigPath, "12345.json");
            if (File.Exists(pathToFile))
                File.Delete(pathToFile);

            var manager = BotManagerTests.CreateBotManager(1);
            manager.ConfigManager.GlobalBotConfig.DefaultBotConfig.Prefix = "??";   //Change a prefix to make sure the bot has loaded the old config
            var bot = manager.ActivateBots(1).FirstOrDefault();
            bot.Dispose();

            manager.ConfigManager.GlobalBotConfig.DefaultBotConfig.Prefix = "!";
            bot = manager.ActivateBots(1).FirstOrDefault(); //restart the bot to re-fetch the config

            BotConfig config = bot.Config;

            Assert.IsTrue(config != null && bot.Config.Prefix == "??");
        }

        [TestMethod()]
        public void LoadExisting_Extensions()
        {
            string pathToFile = Path.Combine(_config.ConfigPath, "12345-e.json");
            if (File.Exists(pathToFile))
                File.Delete(pathToFile);

            var manager = BotManagerTests.CreateBotManager(1);
            var bot = manager.ActivateBots(1, typeof(GlobalBotConfig)).FirstOrDefault();
            (bot.Config.Extension as GlobalBotConfig).DefaultBotConfig.Prefix = "??";
            bot.Dispose();

            bot = manager.ActivateBots(1, typeof(GlobalBotConfig)).FirstOrDefault(); //restart the bot to re-fetch the config

            GlobalBotConfig config = bot.Config.Extension as GlobalBotConfig;

            Assert.IsTrue(config != null && bot.Config.Prefix == "??");
        }

        [TestMethod()]
        public void NullExtension()
        {
            string pathToFile = Path.Combine(_config.ConfigPath, "12345.json");
            if (File.Exists(pathToFile))
                File.Delete(pathToFile);

            var manager = BotManagerTests.CreateBotManager(1);
            var bot = manager.ActivateBots(1).FirstOrDefault();

            BotConfig config = bot.Config;

            Assert.ThrowsException<InvalidOperationException>(delegate { object extObject = config.Extension; });
        }
    }
}