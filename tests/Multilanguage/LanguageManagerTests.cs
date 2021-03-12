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
    [DeploymentItem("Languages\\")]
    public class LanguageManagerTests
    {
        private LanguageManager CreateLanguageManager(DCoreConfig config, string language = "en")
        {
            BotManager manager = new BotManager(new ConfigManager(config), config);
            DiscordBot bot = new DiscordBot(manager, new TokenInfo());
            bot.Config.Language = language;
            LanguageManager languageManager = new LanguageManager(bot, config);

            return languageManager;
        }

        [TestMethod()]
        public void LoadLanguageData_EmptyPath()
        {
            DCoreConfig config = new DCoreConfig { LanguagesPath = "EmptyLanguages\\" };

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

        [TestMethod()]
        public void LoadLanguageData_Load()
        {
            DCoreConfig config = new DCoreConfig();

            //Clear the files if any exist
            string exampleFile = Path.Combine(config.LanguagesPath, "example.json");
            if (File.Exists(exampleFile))
                File.Delete(exampleFile);
            var languageManager = CreateLanguageManager(config);

            languageManager.LoadLanguageData();

            int fileCount = Directory.EnumerateFiles(config.LanguagesPath).ToList().Count;
            Assert.IsTrue(fileCount == 2
                && !File.Exists(exampleFile));
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void GetString_NullOrEmpty(string input)
        {
            DCoreConfig config = new DCoreConfig();
            var languageManager = CreateLanguageManager(config);
            languageManager.LoadLanguageData();

            void get() => languageManager.GetString(input);
            Assert.ThrowsException<ArgumentNullException>(get);
        }

        [TestMethod()]
        public void GetString_MissingLanguage()
        {
            DCoreConfig config = new DCoreConfig();
            var languageManager = CreateLanguageManager(config, "non-existing-language");
            languageManager.LoadLanguageData();

            void get() => languageManager.GetString("example1");
            Assert.ThrowsException<InvalidOperationException>(get);
        }

        [TestMethod()]
        public void GetString_MissingIdentifier()
        {
            DCoreConfig config = new DCoreConfig();
            var languageManager = CreateLanguageManager(config, "en-1");
            languageManager.LoadLanguageData();

            void get() => languageManager.GetString("non-existing-identifier");
            Assert.ThrowsException<ArgumentException>(get);
        }

        [DataTestMethod]
        [DataRow("example1")]
        [DataRow("example2")]
        [DataRow("example3")]
        public void GetString_CorrectData(string identifier)
        {
            DCoreConfig config = new DCoreConfig();
            var languageManager = CreateLanguageManager(config, "en-1");
            languageManager.LoadLanguageData();

            string expected = "This is a test value.";
            string output = languageManager.GetString(identifier);
            Assert.AreEqual(expected, output);
        }
    }
}