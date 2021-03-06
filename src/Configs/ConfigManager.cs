using DCore.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DCore.Configs
{
    /// <summary>
    /// Handles loading config files.
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// Global config used by all instances of <see cref="DiscordBot"/>.
        /// </summary>
        public GlobalBotConfig GlobalBotConfig
        {
            get
            {
                //Load the config if needed
                if (_config == null)
                    _config = LoadConfig(GetPathToGlobalConfig(), typeof(GlobalBotConfig)) as GlobalBotConfig;

                return _config;
            }

            set
            {
                _config = value;
            }
        }
        private GlobalBotConfig _config;

        private DCoreConfig _dcoreConfig;

        /// <summary>
        /// Loads a config of <see cref="Type"/> <paramref name="type"/> from specified path.
        /// </summary>
        /// <param name="path"> The path to load the config file from. </param>
        /// <param name="type"> The <see cref="Type"/> of the config class. </param>
        /// <exception cref="FileNotFoundException"> Thrown when file is not found. </exception>
        /// <returns></returns>
        private object LoadConfig(string path, Type type)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File does not exist.");

            //Attempt to load the file
            string content = File.ReadAllText(path);
            object config = JsonConvert.DeserializeObject(content, type);

            return config;
        }

        /// <summary>
        /// Gets the path to the global config file.
        /// </summary>
        /// <returns> The path to the global config file. </returns>
        private string GetPathToGlobalConfig()
        {
            return Path.Combine(_dcoreConfig.ConfigPath, "global.json");
        }
    }
}
