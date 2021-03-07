using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore.Configs;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DCore.Configs.Tests
{
    [TestClass()]
    public class ConfigManagerTests
    {
        readonly DCoreConfig _config = new DCoreConfig();

        [TestMethod()]
        public void ConfigManager_CreateNew()
        {
            string pathToFile = Path.Combine(_config.ConfigPath, "global.json");
            if (File.Exists(pathToFile))
                File.Delete(pathToFile);
            
            ConfigManager config = new ConfigManager(_config);
            var globalConfig = config.GlobalBotConfig;

            Assert.IsTrue(globalConfig != null && File.Exists(pathToFile));
        }

        [TestMethod()]
        public void ConfigManager_CreateNew_Extension()
        {
            string pathToFile = Path.Combine(_config.ConfigPath, "global-e.json");
            if (File.Exists(pathToFile))
                File.Delete(pathToFile);

            //Use BotConfig as an example extension
            ConfigManager config = new ConfigManager(_config, typeof(BotConfig));
            var globalConfig = config.GlobalBotConfig;

            Assert.IsTrue(globalConfig != null && File.Exists(pathToFile));
        }

        [TestMethod()]
        public void ConfigManager_LoadExisting()
        {
            string pathToFile = Path.Combine(_config.ConfigPath, "global.json");
            if (File.Exists(pathToFile))
                File.Delete(pathToFile);
            _ = new ConfigManager(_config)
            {
                //Create the old config
                GlobalBotConfig = new GlobalBotConfig { UseColoredInputInLogs = false }
            };

            //Reset the object
            ConfigManager config = new ConfigManager(_config);
            var globalConfig = config.GlobalBotConfig;

            Assert.IsTrue(globalConfig.UseColoredInputInLogs == false);
        }

        [TestMethod()]
        public void ConfigManager_LoadExisting_Extension()
        {
            string pathToFile = Path.Combine(_config.ConfigPath, "global-e.json");
            if (File.Exists(pathToFile))
                File.Delete(pathToFile);
            _ = new ConfigManager(_config, typeof(BotConfig))
            {
                //Create the old config
                GlobalBotConfig = new GlobalBotConfig()
            };

            //Reset the object
            ConfigManager config = new ConfigManager(_config, typeof(BotConfig));
            var globalConfig = config.GlobalBotConfig;
            
            Assert.IsTrue(globalConfig.Extension is BotConfig);
        }
    }
}