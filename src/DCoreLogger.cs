using DCore.Enums;
using DCore.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DCore
{
    /// <summary>
    /// Handles logging into console and files.
    /// </summary>
    public class DCoreLogger
    {
        /// <summary>
        /// The bot tied to this Logger.
        /// </summary>
        public DiscordBot Bot { get; set; }

        /// <summary>
        /// Logs information to console.
        /// </summary>
        /// <param name="message"> The message to log. </param>
        public void Info(string message)
        {
            LogInformation(LogType.Info, message);
        }

        /// <summary>
        /// Logs information to console.
        /// </summary>
        /// <param name="message"> The message to log. </param>
        public void Info(object message)
        {
            LogInformation(LogType.Info, message.ToString());
        }

        /// <summary>
        /// Logs debug information to console.
        /// </summary>
        /// <param name="message"> The message to log. </param>
        public void Debug(string message)
        {
            LogInformation(LogType.Debug, message);
        }

        /// <summary>
        /// Logs debug information to console.
        /// </summary>
        /// <param name="message"> The message to log. </param>
        public void Debug(object message)
        {
            LogInformation(LogType.Debug, message.ToString());
        }

        /// <summary>
        /// Logs a warning to console.
        /// </summary>
        /// <param name="message"> The message to log. </param>
        public void Warn(string message)
        {
            LogInformation(LogType.Warn, message);
        }

        /// <summary>
        /// Logs a warning to console.
        /// </summary>
        /// <param name="message"> The message to log. </param>
        public void Warn(object message)
        {
            LogInformation(LogType.Warn, message.ToString());
        }

        /// <summary>
        /// Logs an error to console.
        /// </summary>
        /// <param name="message"> The message to log. </param>
        public void Error(string message)
        {
            LogInformation(LogType.Error, message);
        }

        /// <summary>
        /// Logs an error to console.
        /// </summary>
        /// <param name="message"> The message to log. </param>
        public void Error(object message)
        {
            LogInformation(LogType.Error, message.ToString());
        }

        /// <summary>
        /// Logs a critical error to console.
        /// </summary>
        /// <param name="message"> The message to log. </param>
        public void CriticalError(string message)
        {
            LogInformation(LogType.CriticalError, message);
        }

        /// <summary>
        /// Logs a critical error to console.
        /// </summary>
        /// <param name="message"> The message to log. </param>
        public void CriticalError(object message)
        {
            LogInformation(LogType.CriticalError, message.ToString());
        }



        /// <summary>
        /// Logs a message of type <paramref name="type"/>.
        /// </summary>
        /// <param name="type"> Type of log. </param>
        /// <param name="message"> Message to log. </param>
        public void LogInformation(LogType type, string message)
        {
            //Log the output to console and then to file
            LoggingWriter writer = new LoggingWriter(this);
            writer.WriteToConsole(type, message);

            //Write to the files
            _ = Task.Run(() => writer.WriteToFileAsync(type, message));
        }


        public DCoreLogger(DiscordBot bot)
        {
            Bot = bot;
        }
    }
}
