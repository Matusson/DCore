using DCore.Configs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// Loads the language data into memory.
        /// </summary>
        public void LoadLanguageData()
        {
            //Get all files in the directory
            string path = _config.LanguagesPath;
            var languageFiles = Directory.EnumerateFiles(path).ToList();

            //Enumerate through all of the languages
            foreach (string languageFile in languageFiles)
            {
                //Get the language identifier of the file
                string languageIdentifier = Path.GetFileNameWithoutExtension(languageFile);

                //Read the file and deserialize the data
                string json = File.ReadAllText(languageFile);
                LanguageData languageData = JsonConvert.DeserializeObject<LanguageData>(json);

                //Add to the language data dictionary
                _languages[languageIdentifier] = languageData;
            }
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
