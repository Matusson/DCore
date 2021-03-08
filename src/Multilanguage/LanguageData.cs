using System;
using System.Collections.Generic;
using System.Text;

namespace DCore
{
    /// <summary>
    /// Stores strings for a language.
    /// </summary>
    internal class LanguageData
    {
        /// <summary>
        /// Stores strings for the language. (key is identifier)
        /// </summary>
        internal Dictionary<string, string> Strings = new Dictionary<string, string>();
    }
}
