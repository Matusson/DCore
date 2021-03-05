using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCore;
using DCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace DCore.Tests
{
    [TestClass()]
    public class DCoreLoggerTests
    {
        static readonly string temponaryLogPath = "\\Logs\\combined.log";

        [TestMethod()]
        public async Task LogInformation_FileCreated()
        {
            if (File.Exists(temponaryLogPath))
                File.Delete(temponaryLogPath);

            
            DCoreLogger logger = new DCoreLogger();
            logger.LogInformation(LogType.Info, "TEST");

            //The logger doesn't wait for the writing to complete, so wait a bit
            await Task.Delay(10);

            //TODO: add the config
            Assert.IsTrue(File.Exists(temponaryLogPath));
        }

        [TestMethod()]
        public async Task LogInformation_CorrectContent()
        {
            if (File.Exists(temponaryLogPath))
                File.Delete(temponaryLogPath);

            DCoreLogger logger = new DCoreLogger();
            logger.LogInformation(LogType.Info, "TEST");

            //The logger doesn't wait for the writing to complete, so wait a bit
            await Task.Delay(10);

            string content = File.ReadAllText(temponaryLogPath);
            string expected = $"[INFO]  {DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToLongTimeString()} | TEST";
            Assert.AreEqual(expected, content);
        }

        [TestMethod()]
        public async Task LogInformation_LongInput()
        {
            if (File.Exists(temponaryLogPath))
                File.Delete(temponaryLogPath);

            StringBuilder toWrite = new StringBuilder(10000001);
            for (int i = 0; i < 10000000; i++)
                toWrite.Append("A");

            DCoreLogger logger = new DCoreLogger();
            logger.LogInformation(LogType.Info, toWrite.ToString());
            DateTime startedWriting = DateTime.UtcNow;

            //The logger doesn't wait for the writing to complete, so wait a bit
            await Task.Delay(100);

            string content = File.ReadAllText(temponaryLogPath);
            string expected = $"[INFO]  {startedWriting.ToShortDateString()} {startedWriting.ToLongTimeString()} | {toWrite}";
            Assert.AreEqual(expected, content);
        }

        [TestMethod()]
        public async Task LogInformation_MultipleWrites()
        {
            if (File.Exists(temponaryLogPath))
                File.Delete(temponaryLogPath);

            StringBuilder toWrite = new StringBuilder(1001);
            for (int i = 0; i < 1000; i++)
                toWrite.Append("A");

            DCoreLogger logger = new DCoreLogger();

            int iterations = 10;
            for(int i = 0; i < iterations; i++)
                logger.LogInformation(LogType.Info, toWrite.ToString());

            //The logger doesn't wait for the writing to complete, so wait a bit
            await Task.Delay(5000);

            string[] content = File.ReadAllLines(temponaryLogPath);
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