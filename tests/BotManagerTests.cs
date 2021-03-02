using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using System;
using System.Collections.Generic;
using System.Text;
using DCore.Structs;

namespace DCore.Tests
{
    [TestClass()]
    public class BotManagerTests
    {
        [TestMethod()]
        public void LoadAccounts_UniqueOnly()
        {
            BotManager manager = new BotManager();
            List<TokenInfo> accounts = new List<TokenInfo>
            {
                new TokenInfo(012345, "TOKEN"),
                new TokenInfo(123456, "TOKEN"),
                new TokenInfo(234567, "TOKEN")
            };

            int result = manager.LoadAccounts(accounts);

            Assert.AreEqual(3, result);
        }

        [TestMethod()]
        public void LoadAccounts_DuplicatesOnly()
        {
            BotManager manager = new BotManager();
            List<TokenInfo> accounts = new List<TokenInfo>
            {
                new TokenInfo(012345, "TOKEN"),
                new TokenInfo(012345, "TOKEN"),
                new TokenInfo(012345, "TOKEN")
            };

            int result = manager.LoadAccounts(accounts);

            Assert.AreEqual(1, result);
        }

        [TestMethod()]
        public void LoadAccounts_UniqueDuplicateMix()
        {
            BotManager manager = new BotManager();
            List<TokenInfo> accounts = new List<TokenInfo>
            {
                new TokenInfo(012345, "TOKEN"),
                new TokenInfo(012345, "TOKEN"),
                new TokenInfo(123456, "TOKEN")
            };

            int result = manager.LoadAccounts(accounts);

            Assert.AreEqual(2, result);
        }


        [TestMethod()]
        public void LoadAccounts_TwoLoads_UniqueOnly()
        {
            BotManager manager = new BotManager();
            List<TokenInfo> accounts = new List<TokenInfo>
            {
                new TokenInfo(012345, "TOKEN"),
                new TokenInfo(123456, "TOKEN"),
                new TokenInfo(234567, "TOKEN")
            };

            List<TokenInfo> accounts2 = new List<TokenInfo>
            {
                new TokenInfo(345678, "TOKEN"),
                new TokenInfo(456789, "TOKEN"),
                new TokenInfo(567890, "TOKEN")
            };

            manager.LoadAccounts(accounts);
            manager.LoadAccounts(accounts2);
            int result = manager.TotalBotCount;

            Assert.AreEqual(6, result);
        }

        [TestMethod()]
        public void LoadAccounts_TwoLoads_UniqueDuplicateMix()
        {
            BotManager manager = new BotManager();
            List<TokenInfo> accounts = new List<TokenInfo>
            {
                new TokenInfo(012345, "TOKEN"),
                new TokenInfo(123456, "TOKEN"),
                new TokenInfo(234567, "TOKEN")
            };

            List<TokenInfo> accounts2 = new List<TokenInfo>
            {
                new TokenInfo(345678, "TOKEN"),
                new TokenInfo(234567, "TOKEN"),
                new TokenInfo(012345, "TOKEN")
            };

            manager.LoadAccounts(accounts);
            manager.LoadAccounts(accounts2);
            int result = manager.TotalBotCount;

            Assert.AreEqual(4, result);
        }
    }
}