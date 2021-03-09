using DCore.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCore
{
    /// <summary>
    /// Manages multi-language data.
    /// </summary>
    public class MultilanguageManager
    {
        /// <summary>
        /// Stores language data for every loaded language.
        /// </summary>
        private Dictionary<string, LanguageData> _languages = new Dictionary<string, LanguageData>();

        private DCoreConfig _config;

        public void LoadLanguageData()
        {

        }

        public void GetString()
        {

        }

        /// <summary>
        /// Constructs a <see cref="MultilanguageManager"/> with the specified config.
        /// </summary>
        /// <param name="config"></param>
        public MultilanguageManager(DCoreConfig config)
        {
            _config = config;
        }
    }
}
