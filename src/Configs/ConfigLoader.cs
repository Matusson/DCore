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
        /// Loads the config at specified path of type <paramref name="type"/>.
        /// </summary>
        /// <param name="path"> The path of the config file. </param>
        /// <param name="type"> The <see cref="Type"/> of the config. </param>
        /// <returns> The config object. </returns>
        internal object LoadConfig(string path, Type type)
        {
            object newConfig;

            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            //If the file doesn't exist, try to create one
            if (!File.Exists(path))
            {
                newConfig = Activator.CreateInstance(type);
                SaveConfig(newConfig, path);
            }

            //Attempt to load the file
            else
            {
                string content = File.ReadAllText(path);
                newConfig = JsonConvert.DeserializeObject(content, type);
            }

            return newConfig;
        }

        /// <summary>
        /// Saves the config object to the specified path.
        /// </summary>
        /// <param name="path"> The path to write the config into. </param>
        /// <param name="config"> The config object. </param>
        internal void SaveConfig(object config, string path)
        {
            string content = JsonConvert.SerializeObject(config);

            //Write to the file
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
