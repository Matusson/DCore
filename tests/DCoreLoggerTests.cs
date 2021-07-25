using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using DCore.Configs;
using DCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Moq;

namespace DCore.Tests
{
    [TestClass()]
    public class DCoreLoggerTests
    {
        static readonly string defaultLoggingPath = "Logs\\combined.log";

        private DCoreLogger GetLogger(bool multipleBots = false)
        {
            DCoreConfig config = new DCoreConfig { UseMultipleBots = multipleBots };
            BotManager manager = new BotManager(new ConfigManager(config), config);
            Mock<DiscordBot> bot = new Mock<DiscordBot>(manager, new TokenInfo(12345, "TOKEN"), null, null);
            DCoreLogger logger = new DCoreLogger(bot.Object);

            return logger;
        }

        [TestMethod()]
        public async Task LogInformation_FileCreated()
        {
            if (File.Exists(defaultLoggingPath))
                File.Delete(defaultLoggingPath);

            var logger = GetLogger();
            logger.LogInformation(LogType.Info, "TEST");

            //The logger doesn't wait for the writing to complete, so wait a bit
            await Task.Delay(10);

            Assert.IsTrue(File.Exists(defaultLoggingPath));
        }

        [TestMethod()]
        public async Task LogInformation_FileCreated_MultipleAccounts()
        {
            string expectedLogFilePath = "Logs\\12345.log";
            if (File.Exists(defaultLoggingPath))
                File.Delete(defaultLoggingPath);

            if (File.Exists(expectedLogFilePath))
                File.Delete(expectedLogFilePath);

            var logger = GetLogger(true);
            logger.LogInformation(LogType.Info, "TEST");

            //The logger doesn't wait for the writing to complete, so wait a bit
            await Task.Delay(20);

            Assert.IsTrue(File.Exists(expectedLogFilePath));
        }

        [TestMethod()]
        public async Task LogInformation_FileCreated_SingleAccount()
        {
            string expectedLogFilePath = "Logs\\12345.log";
            if (File.Exists(defaultLoggingPath))
                File.Delete(defaultLoggingPath);

            if (File.Exists(expectedLogFilePath))
                File.Delete(expectedLogFilePath);

            var logger = GetLogger();
            logger.LogInformation(LogType.Info, "TEST");

            //The logger doesn't wait for the writing to complete, so wait a bit
            await Task.Delay(20);

            Assert.IsTrue(!File.Exists(expectedLogFilePath));
        }

        [TestMethod()]
        public async Task LogInformation_CorrectContent()
        {
            if (File.Exists(defaultLoggingPath))
                File.Delete(defaultLoggingPath);

            var logger = GetLogger();
            logger.LogInformation(LogType.Info, "TEST");

            //The logger doesn't wait for the writing to complete, so wait a bit
            await Task.Delay(10);

            string content = File.ReadAllText(defaultLoggingPath);
            string expected = $"[INFO]  {DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToLongTimeString()} | TEST";
            Assert.AreEqual(expected, content);
        }

        [TestMethod()]
        public async Task LogInformation_CorrectContent_MultipleBots()
        {
            if (File.Exists(defaultLoggingPath))
                File.Delete(defaultLoggingPath);

            var logger = GetLogger(true);
            logger.LogInformation(LogType.Info, "TEST");

            //The logger doesn't wait for the writing to complete, so wait a bit
            await Task.Delay(10);

            string content = File.ReadAllText(defaultLoggingPath);
            string expected = $"[INFO]  [12345] {DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToLongTimeString()} | TEST";
            Assert.AreEqual(expected, content);
        }

        [TestMethod()]
        public async Task LogInformation_LongInput()
        {
            if (File.Exists(defaultLoggingPath))
                File.Delete(defaultLoggingPath);

            StringBuilder toWrite = new StringBuilder(10000001);
            for (int i = 0; i < 10000000; i++)
                toWrite.Append("A");

            var logger = GetLogger();
            logger.LogInformation(LogType.Info, toWrite.ToString());
            DateTime startedWriting = DateTime.UtcNow;

            //The logger doesn't wait for the writing to complete, so wait a bit
            await Task.Delay(100);

            string content = File.ReadAllText(defaultLoggingPath);
            string expected = $"[INFO]  {startedWriting.ToShortDateString()} {startedWriting.ToLongTimeString()} | {toWrite}";
            Assert.AreEqual(expected, content);
        }

        [TestMethod()]
        public async Task LogInformation_MultipleWrites()
        {
            if (File.Exists(defaultLoggingPath))
                File.Delete(defaultLoggingPath);

            StringBuilder toWrite = new StringBuilder(101);
            for (int i = 0; i < 100; i++)
                toWrite.Append("A");

            var logger = GetLogger();

            int iterations = 5;
            for (int i = 0; i < iterations; i++)
            {
                logger.LogInformation(LogType.Info, toWrite.ToString());
                await Task.Delay(5);
            }

            //The logger doesn't wait for the writing to complete, so wait a bit
            await Task.Delay(500);

            string[] content = File.ReadAllLines(defaultLoggingPath);
            Assert.AreEqual(iterations, content.Length);
        }

        [TestCleanup()]
        public async Task Cleanup()
        {
            //Some wait is needed to make sure the file writing isn't overloaded
            await Task.Delay(500);
        }
    }
}