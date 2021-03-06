using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DCore.Configs
{
    /// <summary>
    /// Helper class for loading configs.
    /// </summary>
    internal class ConfigLoader
    {
        private readonly DCoreConfig _dcoreConfig;

        
        /// <summary>
        /// Loads the global config file.
        /// </summary>
        /// <returns> The loaded config file. </returns>
        internal GlobalBotConfig LoadGlobalConfig()
        {
            string pathToGlobalConfig = GetPathToGlobalConfig();
            GlobalBotConfig newConfig;

            //If the file doesn't exist, try to create one
            if (!File.Exists(pathToGlobalConfig))
            {
                newConfig = new GlobalBotConfig();
            }

            //Attempt to load the file
            else
            {
                string content = File.ReadAllText(pathToGlobalConfig);
                newConfig = JsonConvert.DeserializeObject<GlobalBotConfig>(content);
            }

            return newConfig;
        }

        /// <summary>
        /// Writes the global config to HDD.
        /// </summary>
        /// <param name="config"></param>
        internal void SaveGlobalConfig(GlobalBotConfig config)
        {
            string content = JsonConvert.SerializeObject(config);

            //Write to the file
            string path = GetPathToGlobalConfig();
            File.WriteAllText(path, content);
        }

        /// <summary>
        /// Gets the path to the global config file.
        /// </summary>
        /// <returns> The path to the global config file. </returns>
        internal string GetPathToGlobalConfig()
        {
            return Path.Combine(_dcoreConfig.ConfigPath, "global.json");
        }

        /// <summary>
        /// Gets the path to the config file for the specified <see cref="DiscordBot"/>.
        /// </summary>
        /// <param name="bot"> The bot account to get config for. </param>
        /// <returns> The path to the specified config file. </returns>
        internal string GetPathToBotConfig(DiscordBot bot)
        {
            return GetPathToBotConfig(bot.TokenInfo.id);
        }

        /// <summary>
        /// Gets the path to the config file for the specified ID.
        /// </summary>
        /// <param name="id"> The bot account ID to get config for. </param>
        /// <returns> The path to the specified config file. </returns>
        internal string GetPathToBotConfig(ulong id)
        {
            return Path.Combine(_dcoreConfig.ConfigPath, $"{id}.json");
        }

        /// <summary>
        /// Constructs a ConfigLoader with the specified config.
        /// </summary>
        /// <param name="dcoreConfig"></param>
        internal ConfigLoader(DCoreConfig dcoreConfig)
        {
            _dcoreConfig = dcoreConfig;
        }
    }
}
