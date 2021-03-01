using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DCore.Tests
{
    [TestClass()]
    [DeploymentItem("TokenFiles\\", "TokenFiles")]
    public class BotManagerTests
    {
        [TestMethod()]
        public void LoadAccounts_NoFile()
        {
            var manager = new BotManager();

            void load() => manager.LoadAccounts("TokenFiles\\tokensNotExisting.txt");

            Assert.ThrowsException<FileNotFoundException>(load);
        }

        [TestMethod()]
        public void LoadAccounts_EmptyFile()
        {
            var manager = new BotManager();

            void load() => manager.LoadAccounts("TokenFiles\\tokensEmpty.txt");

            Assert.ThrowsException<ArgumentException>(load);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void LoadAccounts_InvalidParameterCount(int testId)
        {
            var manager = new BotManager();

            void load() => manager.LoadAccounts($"TokenFiles\\tokensParameterCount{testId}.txt");

            Assert.ThrowsException<ArgumentException>(load);
        }

        [TestMethod()]
        public void LoadAccounts_InvalidDataType()
        {
            var manager = new BotManager();

            void load() => manager.LoadAccounts("TokenFiles\\tokensDataType1.txt");

            Assert.ThrowsException<ArgumentException>(load);
        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 1)]
        [DataRow(3, 1)]
        [DataRow(4, 3)]
        public void LoadAccounts_CorrectResult(int testId, int expectedBotCount)
        {
            var manager = new BotManager();

            int actualBotCount = manager.LoadAccounts($"TokenFiles\\tokensCorrect{testId}.txt");

            Assert.AreEqual(expectedBotCount, actualBotCount);
        }
    }
}