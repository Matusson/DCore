using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using DCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DCore.Tests
{
    [TestClass()]
    [DeploymentItem("TokenFiles\\", "TokenFiles")]
    public class BotAccountLoaderTests
    {
        [TestMethod()]
        public void LoadAccounts_NoFile()
        {
            var loader = new BotAccountLoader();

            void load() => loader.LoadAccountsFromFile("TokenFiles\\tokensNotExisting.txt");

            Assert.ThrowsException<FileNotFoundException>(load);
        }

        [TestMethod()]
        public void LoadAccounts_EmptyFile()
        {
            var loader = new BotAccountLoader();

            void load() => loader.LoadAccountsFromFile("TokenFiles\\tokensEmpty.txt");

            Assert.ThrowsException<ArgumentException>(load);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void LoadAccounts_InvalidParameterCount(int testId)
        {
            var loader = new BotAccountLoader();

            void load() => loader.LoadAccountsFromFile($"TokenFiles\\tokensParameterCount{testId}.txt");

            Assert.ThrowsException<ArgumentException>(load);
        }

        [TestMethod()]
        public void LoadAccounts_InvalidDataType()
        {
            var loader = new BotAccountLoader();

            void load() => loader.LoadAccountsFromFile("TokenFiles\\tokensDataType1.txt");

            Assert.ThrowsException<ArgumentException>(load);
        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 1)]
        [DataRow(3, 1)]
        [DataRow(4, 3)]
        public void LoadAccounts_CorrectResult(int testId, int expectedBotCount)
        {
            var loader = new BotAccountLoader();

            int actualBotCount = loader.LoadAccountsFromFile($"TokenFiles\\tokensCorrect{testId}.txt").Count;

            Assert.AreEqual(expectedBotCount, actualBotCount);
        }
    }
}