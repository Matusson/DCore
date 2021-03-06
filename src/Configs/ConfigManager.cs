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
        /// Loads a config of <see cref="Type"/> <paramref name="type"/> from specified path.
        /// </summary>
        /// <param name="path"> The path to load the config file from. </param>
        /// <param name="type"> The <see cref="Type"/> of the config class. </param>
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
    }
}
