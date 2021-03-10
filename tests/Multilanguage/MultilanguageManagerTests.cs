using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using System;
using System.Collections.Generic;
using System.Text;
using DCore.Configs;
using Moq;
using System.IO;
using System.Linq;

namespace DCore.Tests
{
    [TestClass()]
    public class MultilanguageManagerTests
    {
        private MultilanguageManager CreateLanguageManager(DCoreConfig config)
        {
            BotManager manager = new BotManager(new ConfigManager(config), config);
            DiscordBot bot = new DiscordBot(manager, new TokenInfo());
            MultilanguageManager languageManager = new MultilanguageManager(bot, config);

            return languageManager;
        }

        [TestMethod()]
        public void LoadLanguageData_EmptyPath()
        {
            DCoreConfig config = new DCoreConfig();

            //Clear the files if any exist
            if (Directory.Exists(config.LanguagesPath))
            {
                var oldFiles = Directory.EnumerateFiles(config.LanguagesPath).ToList();
                oldFiles.ForEach(x => File.Delete(x));
            }
            var languageManager = CreateLanguageManager(config);

            languageManager.LoadLanguageData();

            int fileCount = Directory.EnumerateFiles(config.LanguagesPath).ToList().Count;
            string expectedFile = Path.Combine(config.LanguagesPath, "example.json");
            Assert.IsTrue(fileCount == 1 
                && File.Exists(expectedFile));
        }
    }
}