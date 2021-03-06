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
                {
                    _config = LoadGlobalConfig();
                }

                return _config;
            }

            set
            {
                _config = value;
                SaveGlobalConfig(_config);
            }
        }
        private GlobalBotConfig _config;
        private readonly ConfigLoader _loader;

        /// <summary>
        /// Loads global config from JSON.
        /// </summary>
        /// <returns></returns>
        internal GlobalBotConfig LoadGlobalConfig()
        {
            string path = _loader.GetPathToGlobalConfig();
            return _loader.LoadConfig(path, typeof(GlobalBotConfig)) as GlobalBotConfig;
        }

        /// <summary>
        /// Writes the global config to HDD.
        /// </summary>
        /// <param name="config"> The config object. </param>
        internal void SaveGlobalConfig(GlobalBotConfig config)
        {
            string path = _loader.GetPathToGlobalConfig();
            _loader.SaveConfig(config, path);
        }

        /// <summary>
        /// Constructs a <see cref="ConfigManager"/> with the specified DCore config.
        /// </summary>
        /// <param name="dcoreConfig"></param>
        public ConfigManager(DCoreConfig dcoreConfig)
        {
            _loader = new ConfigLoader(dcoreConfig);
        }
    }
}
