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
    public class MultilanguageManager
    {
        /// <summary>
        /// Stores language data for every loaded language.
        /// </summary>
        public Dictionary<string, LanguageData> Languages = new Dictionary<string, LanguageData>();

        private readonly DCoreConfig _config;

        /// <summary>
        /// Loads the language data into memory.
        /// </summary>
        public void LoadLanguageData()
        {
            //Get all files in the directory
            string path = _config.LanguagesPath;
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
                LanguageData languageData = JsonConvert.DeserializeObject<LanguageData>(json);

                //Add to the language data dictionary
                Languages[languageIdentifier] = languageData;
            }
        }

        public void GetString()
        {

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
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            //Save the data
            File.WriteAllText(path, json);
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
