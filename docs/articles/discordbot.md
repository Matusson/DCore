In the previous guide, you used BotManager to request accounts. This guide will cover how to use the DiscordBot class, created as a result.

# Basic Features
Use DiscordBot.StartAsync(), StopAsync(), and RestartAsync() for those basic functions:
```cs
await bot.StartAsync();
await bot.StopAsync();
await bot.RestartAsync();
```

You can fetch the underlying Discord.NET client, the logger for this DiscordBot, the bot's config, the language manager, and the BotManager that created this DiscordBot.
```cs
 DiscordSocketClient discordClient = bot.Client;
DCoreLogger logger = bot.Logger;
BotConfig botConfig = bot.Config;

BotManager manager = bot.Manager;
LanguageManager languagesManager = bot.Languages;
```

# Logger
DCore Logger is the basic logging solution. It supports logging to console (with color depending on log severity) and to a text file.
It can be configured with DCore Config used while creating the BotManager (or when using AddDCore method). Its features are self-explanatory.

# Bot Config
The bot config contains basic variables that might be useful to all bots, and a save function. However each bot is different, so a config extension can be added. 
The extension is a custom class that is serialized alongside the config, and can be accessed by using BotConfig.Extension. The Type of extension should be specified in BotManager.RequestBots().

```cs
var bots = botManager.ActivateBots(1, typeof(ExampleConfigExtension));
var botConfig = bots.First().Config;
string example = (botConfig.Extension as ExampleConfigExtension).ExampleCustomVariable;
```

> [!NOTE]
> Please note the distinction between GlobalBotConfig, BotConfig, and DCoreConfig. They are explained more thoroughly in the Configuration article.

# Language Manager
The Language Manager allows you to use multiple languages in your bot. It uses JSON files in the format of key-value pairs for each language, which should be placed in the LanguagesPath directory specified in the config.
The built-in way to handle languages requires you to set a language in the Bot Config, and then use LanguageManager.GetString(). This will fetch the specified string in bot's language.
The recommended way to handle the language files is to create them in the project itself, and then use Copy To Output Directory option.