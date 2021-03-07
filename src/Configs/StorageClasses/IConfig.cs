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
        object Extension { get; set; }
    }
}
