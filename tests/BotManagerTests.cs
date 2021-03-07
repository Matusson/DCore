using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using System;
using System.Collections.Generic;
using System.Text;
using DCore.Configs;
using DCore.Structs;
using System.Linq;

namespace DCore.Tests
{
    [TestClass()]
    public class BotManagerTests
    {
        [TestMethod()]
        public void LoadAccounts_UniqueOnly()
        {
            BotManager manager = new BotManager(new DCoreConfig { UseMultipleBots = true });
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
            BotManager manager = new BotManager(new DCoreConfig { UseMultipleBots = true });
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
            BotManager manager = new BotManager(new DCoreConfig { UseMultipleBots = true });
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
            BotManager manager = new BotManager(new DCoreConfig { UseMultipleBots = true });
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
            BotManager manager = new BotManager(new DCoreConfig { UseMultipleBots = true });
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

        [TestMethod()]
        public void LoadAccounts_UseMultipleBotsFalse()
        {
            BotManager manager = new BotManager(new DCoreConfig { UseMultipleBots = false });
            List<TokenInfo> accounts = new List<TokenInfo>
            {
                new TokenInfo(012345, "TOKEN"),
                new TokenInfo(123456, "TOKEN"),
                new TokenInfo(234567, "TOKEN")
            };

            void load() => manager.LoadAccounts(accounts);

            Assert.ThrowsException<InvalidOperationException>(load);
        }


        [DataTestMethod]
        [DataRow(1, 0, 1)]
        [DataRow(3, 3, 0)]
        [DataRow(3, 2, 1)]
        public void AvailableBotAccounts(int total, int activated, int expected)
        {
            BotManager manager = CreateBotManager(total);

            if (activated > 0)
                manager.ActivateBots(activated);
            int result = manager.AvailableBotCount;

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(10)]
        public void ActivateBots_IncorrectParameter(int parameter)
        {
            BotManager manager = CreateBotManager(5);

            void activate() => manager.ActivateBots(parameter);

            Assert.ThrowsException<ArgumentOutOfRangeException>(activate);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(5)]
        public void ActivateBots_Single(int toActivate)
        {
            BotManager manager = CreateBotManager(5);

            int result = manager.ActivateBots(toActivate).Count;

            Assert.AreEqual(toActivate, result);
        }

        [DataTestMethod]
        [DataRow(1, 2)]
        [DataRow(4, 1)]
        public void ActivateBots_DoubleCorrect_Count(int toActivateFirst, int toActivateSecond)
        {
            BotManager manager = CreateBotManager(5);

            int result = manager.ActivateBots(toActivateFirst).Count;
            result += manager.ActivateBots(toActivateSecond).Count;

            Assert.AreEqual(toActivateFirst + toActivateSecond, result);
        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(1, 2)]
        [DataRow(4, 1)]
        public void ActivateBots_DoubleCorrect_IsUnique(int toActivateFirst, int toActivateSecond)
        {
            BotManager manager = CreateBotManager(5);

            var bots1 = manager.ActivateBots(toActivateFirst);
            var bots2 = manager.ActivateBots(toActivateSecond);

            //Ensure that the tokens aren't duplicated
            bool areUnique = bots1.All(x => bots2.Where(y => y.TokenInfo == x.TokenInfo).Count() == 0);
            Assert.IsTrue(areUnique);
        }

        [DataTestMethod]
        [DataRow(1, 5)]
        [DataRow(5, 5)]
        public void ActivateBots_DoubleErorr(int toActivateFirst, int toActivateSecond)
        {
            BotManager manager = CreateBotManager(5);

            manager.ActivateBots(toActivateFirst);
            void activate() => manager.ActivateBots(toActivateSecond);

            Assert.ThrowsException<ArgumentOutOfRangeException>(activate);
        }


        [DataTestMethod]
        [DataRow(1)]
        [DataRow(5)]
        public void GetAllBotsTest(int botCount)
        {
            BotManager manager = CreateBotManager(5);
            manager.ActivateBots(botCount);

            var result = manager.GetAllBots().Count();

            Assert.AreEqual(botCount, result);
        }

        [TestMethod]
        public void GetBot_ID()
        {
            BotManager manager = CreateBotManager(5);
            manager.ActivateBots(5);

            var result = manager.GetBot(012345);

            Assert.AreEqual(result.TokenInfo.id, (ulong)012345);
        }

        [TestMethod]
        public void GetBot_ID_Error()
        {
            BotManager manager = CreateBotManager(5);
            manager.ActivateBots(5);

            void get() => manager.GetBot(98765);

            Assert.ThrowsException<ArgumentException>(get);
        }

        [TestMethod]
        public void GetBot_Token()
        {
            BotManager manager = CreateBotManager(5);
            manager.ActivateBots(5);
            TokenInfo info = new TokenInfo(012345, "TOKEN");

            var result = manager.GetBot(info);

            Assert.AreEqual(result.TokenInfo, info);
        }

        [TestMethod]
        public void GetBot_Token_Error()
        {
            BotManager manager = CreateBotManager(5);
            manager.ActivateBots(5);
            TokenInfo info = new TokenInfo(98765, "TOKEN");

            void get() => manager.GetBot(info);

            Assert.ThrowsException<ArgumentException>(get);
        }

        /// <summary>
        /// Creates a bot manager with x arguments. Must be less or equal 5.
        /// </summary>
        /// <param name="accountCount"> The amount of accounts to initalize with. Must be less or equal 5. </param>
        /// <returns></returns>
        public static BotManager CreateBotManager(int accountCount)
        {
            BotManager manager = new BotManager(new DCoreConfig { UseMultipleBots = true });
            List<TokenInfo> accounts = new List<TokenInfo>
            {
                new TokenInfo(012345, "TOKEN"),
                new TokenInfo(123456, "TOKEN"),
                new TokenInfo(234567, "TOKEN"),
                new TokenInfo(345678, "TOKEN"),
                new TokenInfo(456789, "TOKEN")
            }.Take(accountCount).ToList();

            manager.LoadAccounts(accounts);
            return manager;
        }
    }
}