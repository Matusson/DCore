using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Tests
{
    /* WARNING!
     * This test class requires an ID and token to be provided in the TestToken.txt file.
     * Otherwise the bot will not establish a connection and the tests will fail.
     */

    [TestClass()]
    [DeploymentItem("TestToken.txt")]
    public class DiscordBotTests
    {
        [TestMethod()]
        public void StartAsync_NoConfig()
        {
            BotManager manager = new BotManager();
            manager.LoadAccountsFromFile("TestToken.txt");

        }
    }
}