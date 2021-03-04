using DCore.Enums;
using DCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCore
{
    public class DCoreLogger
    {
        public void LogInformation(LogType type, string message)
        {
            //Log the output to console and then to file
            LoggingWriter writer = new LoggingWriter();
            writer.WriteToConsole(type, message);
            writer.WriteToFile(type, message);
        }
    }
}
