using DCore.Enums;
using DCore.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DCore
{
    public class DCoreLogger
    {
        public void LogInformation(LogType type, string message)
        {
            //TODO:Make this configurable
            string pathToLogs = "\\logs";

            //Log the output to console and then to file
            LoggingWriter writer = new LoggingWriter();
            writer.WriteToConsole(type, message);

            //Write the combined log
            string combinedLogPath = Path.Combine(pathToLogs, "combined.log");
            _ = Task.Run(() => writer.WriteToFileAsync(type, combinedLogPath, message));

            //TODO: If using multiple bots, write to the separated log as well
        }
    }
}
