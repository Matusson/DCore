In the previous guide, you loaded bot information into BotManager. This guide will cover how to use the DiscordBot class, created as a result.

# Fetching bots from BotManager
BotManager.ActivateBots() will return a list of bots activated  by that action. However there are other ways to fetch the bots from BotManager. 
You can use BotManager.GetAllBots() to fetch all currently activated bots:
```cs
//This list will contain the list of all bot accounts.
List<DiscordBot> bots = botManager.GetAllBots();
```
or you can use GetBot() to fetch only the bot with the specified ID or TokenInfo:
```cs
//This will be the bot with this ID
ulong botId = 12345678;
DiscordBot bot = botManager.GetBot(botId);
```

# DiscordBot Features
Use DiscordBot.StartAsync(), StopAsync(), and RestartAsync() for those basic functions:
```cs
await bot.StartAsync();
await bot.StopAsync();
await bot.RestartAsync();
```

You can fetch the underlying Discord.NET client, the logger for this DiscordBot, the bot's config, the BotManager that created this DiscordBot, and the Language manager.
```cs
 DiscordSocketClient discordClient = bot.Client;
DCoreLogger logger = bot.Logger;
BotConfig botConfig = bot.Config;

BotManager manager = bot.Manager;
LanguageManager languagesManager = bot.Languages;
```

## DCore Logger
DCore Logger is the basic logging solution. It supports logging to console (with color depending on log severity) and to a text file.
It can be configured with DCore Config used while creating the BotManager (or when using AddDCore method).

## Bot Config
The bot config contains basic variables that might be useful to all bots, and a save function. However each bot is different, so a config extension can be added. 
The extension is a custom class that is serialized alongside the config, and can be accessed by using BotConfig.Extension. The Type of extension should be specified in BotManager.ActivateBots().

```cs
var bots = botManager.ActivateBots(1, typeof(ExampleConfigExtension));
var botConfig = bots.First().Config;
string example = (botConfig.Extension as ExampleConfigExtension).ExampleCustomVariable;
```