using DCore.Configs;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DCore.Interfaces
{
    /// <summary>
    /// Represents a generic bot account.
    /// </summary>
    public interface IBot
    {
        DiscordBotConfig Config { get; set; }

        Task StartAsync(DiscordSocketConfig config = null);

        Task StopAsync();

        Task RestartAsync();
    }
}
