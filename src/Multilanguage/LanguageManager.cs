﻿using DCore.Configs;
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
    public class LanguageManager
    {
        /// <summary>
        /// Stores language data for every loaded language.
        /// </summary>
        public Dictionary<string, LanguageData> Languages = new Dictionary<string, LanguageData>();

        private readonly DiscordBot _bot;
        private readonly DCoreConfig _config;

        /// <summary>
        /// Loads the language data into memory.
        /// </summary>
        public void ReloadLanguageData()
        {
            //Get all files in the directory
            string path = _config.LanguagesPath;

            //Ensure the path exists
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var languageFiles = Directory.EnumerateFiles(path).ToList();

            //If there are no files, create an example one
            if (languageFiles.Count == 0)
            {
                CreateExampleLanguageFile();
                return;
            }

            //Enumerate through all of the languages
            foreach (string languageFile in languageFiles)
            {
                //Get the language identifier of the file
                string languageIdentifier = Path.GetFileNameWithoutExtension(languageFile);

                //Read the file and deserialize the data
                string json = File.ReadAllText(languageFile);
                Dictionary<string, string> strings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                LanguageData languageData = new LanguageData{ Strings = strings };

                //Add to the language data dictionary
                Languages[languageIdentifier] = languageData;
            }
        }

        /// <summary>
        /// Fetches a string for the bot's language.
        /// </summary>
        /// <param name="identifier"> The string identifier. </param>
        /// <returns> The translated string. </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="identifier"/> is null or empty. </exception>
        /// <exception cref="ArgumentException"> Thrown when <paramref name="identifier"/> is not found in the language file. </exception>
        /// <exception cref="InvalidOperationException"> Thrown when <see cref="DiscordBot"/>'s language is not available. </exception>
        public string GetString(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException("Identifier can't be null or empty.");

            //Check if the currently set language exists in the data
            if (!Languages.ContainsKey(_bot.Config.Language))
                throw new InvalidOperationException($"Language \"{_bot.Config.Language}\" does not exist in the loaded language files.");

            LanguageData data = Languages[_bot.Config.Language];

            //Check for value
            if (!data.Strings.ContainsKey(identifier))
                throw new ArgumentException($"String with identifier \"{identifier}\" was not found in the language file.");

            return data.Strings[identifier];
        }

        /// <summary>
        /// Creates a example language file and saves it to the language files directory.
        /// </summary>
        private void CreateExampleLanguageFile()
        {
            string filename = "example.json";
            string path = Path.Combine(_config.LanguagesPath, filename);

            //Create an object that would hold some example data to make creating initial files simpler
            LanguageData data = new LanguageData()
            {
                Strings = new Dictionary<string, string>()
                {
                    { "example", "This is an example value."},
                    { "example2", "If you haven't already, you should copy this file to the project directory, " +
                    "and use it as a base for language files."},
                    { "example3", "For these files to work correctly in build, make sure to " +
                    "set \"Copy to build directory\" to a value different than \"Do not copy\"."}
                }
            };
            string json = JsonConvert.SerializeObject(data.Strings, Formatting.Indented);

            //Save the data
            File.WriteAllText(path, json);
        }


        /// <summary>
        /// Constructs a <see cref="LanguageManager"/> with the specified config.
        /// </summary>
        /// <param name="bot"> <see cref="DiscordBot"/> linked to this <see cref="LanguageManager"/> instance. </param>
        /// <param name="config"> The config to use. </param>
        public LanguageManager(DiscordBot bot, DCoreConfig config)
        {
            _bot = bot;
            _config = config;

            //Discover language data
            ReloadLanguageData();
        }
    }
}
