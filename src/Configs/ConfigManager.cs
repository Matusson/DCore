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
                    _config = _loader.LoadGlobalConfig();
                }

                return _config;
            }

            set
            {
                _config = value;
            }
        }
        private GlobalBotConfig _config;
        private readonly ConfigLoader _loader;

        
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
