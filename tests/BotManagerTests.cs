using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DCore.Tests
{
    [TestClass()]
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
        [DeploymentItem("TokenFiles\\tokensEmpty.txt", "TokenFiles")]
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
        [DeploymentItem("TokenFiles\\tokensParameterCount1.txt", "TokenFiles")]
        [DeploymentItem("TokenFiles\\tokensParameterCount2.txt", "TokenFiles")]
        [DeploymentItem("TokenFiles\\tokensParameterCount3.txt", "TokenFiles")]
        public void LoadAccounts_InvalidParameterCount(int testId)
        {
            var manager = new BotManager();

            void load() => manager.LoadAccounts($"TokenFiles\\tokensParameterCount{testId}.txt");

            Assert.ThrowsException<ArgumentException>(load);
        }
    }
}