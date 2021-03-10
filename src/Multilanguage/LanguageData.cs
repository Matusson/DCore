using System;
using System.Collections.Generic;
using System.Text;

namespace DCore
{
    /// <summary>
    /// Stores strings for a language.
    /// </summary>
    public class LanguageData
    {
        /// <summary>
        /// Stores strings for the language. (key is identifier)
        /// </summary>
        public Dictionary<string, string> Strings = new Dictionary<string, string>();
    }
}
