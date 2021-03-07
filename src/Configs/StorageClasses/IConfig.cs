using System;
using System.Collections.Generic;
using System.Text;

namespace DCore.Configs
{
    internal interface IConfig
    {
        /// <summary>
        /// Class to use as an Extension to the config.
        /// </summary>
        /// <exception cref="InvalidOperationException"> 
        /// Thrown when fetching the Extension object without initializing it when activating the bot. </exception>
        object Extension { get; set; }

        /// <summary>
        /// Checks whether Extension is null.
        /// </summary>
        bool IsExtensionNull { get; }
    }
}
