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
        /// Loads the config at specified path of type <paramref name="configType"/>.
        /// </summary>
        /// <param name="path"> The path of the config file. </param>
        /// <param name="configType"> The <see cref="Type"/> of the config. </param>
        /// <returns> The config object. </returns>
        internal object LoadConfig(string path, Type configType, Type extensionType = null)
        {
            object newConfig;

            string directory = new FileInfo(path).Directory.FullName;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            //If the file doesn't exist, try to create one
            if (!File.Exists(path))
            {
                return null;
            }

            //Attempt to load the file
            else
            {
                string content = File.ReadAllText(path);
                newConfig = JsonConvert.DeserializeObject(content, configType);
            }

            //Load the extension object if needed
            if (extensionType != null)
            {
                object extension = LoadExtensionObject(path, extensionType);
                (newConfig as IConfig).Extension = extension;
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

            //Attempt to save the config if needed
            SaveExtensionObject(config, path);
        }

        /// <summary>
        /// Attempts to load the extension object.
        /// </summary>
        /// <param name="originalPath"> The path leading to the original config file. </param>
        /// <param name="extensionType"> The <see cref="Type"/> of the extension object. </param>
        /// <returns></returns>
        private object LoadExtensionObject(string originalPath, Type extensionType)
        {
            object newConfigExtension;

            //Find the new path - it's the same as earlier, but with -e suffix
            string path = GetPathToExtension(originalPath);

            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            //If the file doesn't exist, try to create one
            if (!File.Exists(path))
            {
                newConfigExtension = Activator.CreateInstance(extensionType);
                SaveConfig(newConfigExtension, path);
            }

            //Attempt to load the file
            else
            {
                string content = File.ReadAllText(path);
                newConfigExtension = JsonConvert.DeserializeObject(content, extensionType);
            }

            return newConfigExtension;
        }

        /// <summary>
        /// Saves the config object to the specified path.
        /// </summary>
        /// <param name="path"> The path leading to the original config file. </param>
        /// <param name="config"> The config object. </param>
        /// <exception cref="ArgumentException"> Thrown when config object does not implement <see cref="IConfig"/>. </exception>
        private void SaveExtensionObject(object config, string originalPath)
        {
            string path = GetPathToExtension(originalPath);

            if (!(config is IConfig))
                throw new ArgumentException("config object does not implement the IConfig interface.");

            object extensionObject = (config as IConfig).Extension;
            if (extensionObject == null)
                return;

            string content = JsonConvert.SerializeObject(extensionObject);

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
        /// Gets the path to the extension object for the config at the specified path.
        /// </summary>
        /// <param name="originalPath"> The path of the original config file. </param>
        /// <returns> The path of the extension file. </returns>
        private string GetPathToExtension(string originalPath)
        {
            return Path.Combine(Path.GetDirectoryName(originalPath), Path.GetFileNameWithoutExtension(originalPath)) 
                + "-e" + Path.GetExtension(originalPath);
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
