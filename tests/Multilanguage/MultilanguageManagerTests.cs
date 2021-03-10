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
        [TestMethod()]
        public void LoadLanguageData_EmptyPath()
        {
            DCoreConfig config = new DCoreConfig();

            //Clear the files if any exist
            var oldFiles = Directory.EnumerateFiles(config.LanguagesPath).ToList();
            oldFiles.ForEach(x => File.Delete(x));

            Mock<DiscordBot> bot = new Mock<DiscordBot>();
            bot.SetupGet(x => x.Config.Language).Returns("en");
            MultilanguageManager manager = new MultilanguageManager(bot.Object, config);

            manager.LoadLanguageData();

            int fileCount = Directory.EnumerateFiles(config.LanguagesPath).ToList().Count;
            string expectedFile = Path.Combine(config.LanguagesPath, "example.json");
            Assert.IsTrue(fileCount == 1 
                && File.Exists(expectedFile));
        }
    }
}