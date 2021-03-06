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
        /// Loads a config of <see cref="Type"/> <paramref name="type"/> from specified path.
        /// </summary>
        /// <param name="path"> The path to load the config file from. </param>
        /// <param name="type"> The <see cref="Type"/> of the config class. </param>
        /// <exception cref="FileNotFoundException"> Thrown when file is not found. </exception>
        /// <returns></returns>
        internal object LoadConfig(string path, Type type)
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
        internal string GetPathToGlobalConfig()
        {
            return Path.Combine(_dcoreConfig.ConfigPath, "global.json");
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
