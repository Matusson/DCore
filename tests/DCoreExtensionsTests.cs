using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using DCore.Configs;
using DCore.Helpers;
using System.IO;

namespace DCore.Tests
{
    [TestClass()]
    [DeploymentItem("TokenFiles\\", "TokenFiles")]
    public class DCoreExtensionTests
    {
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void LoadAccountsFromFile_InvalidFiles(int testId)
        {
            var manager = new BotManager(new DCoreConfig { UseMultipleBots = true });

            void load() => manager.LoadAccountsFromFile($"TokenFiles\\tokensParameterCount{testId}.txt");

            Assert.ThrowsException<ArgumentException>(load);
        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 1)]
        [DataRow(3, 1)]
        [DataRow(4, 3)]
        public void LoadAccountsFromFile_CorrectResult(int testId, int expectedBotCount)
        {
            var manager = new BotManager(new DCoreConfig { UseMultipleBots = true });

            int actualBotCount = manager.LoadAccountsFromFile($"TokenFiles\\tokensCorrect{testId}.txt");

            Assert.AreEqual(expectedBotCount, actualBotCount);
        }
    }
}