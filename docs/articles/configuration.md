This guide goes more in-depth on how to use the configuration functions in DCore.
It only explains the general concepts, specific settings are documented in the API portion of this documentation (and available via Intellisense).

# Config Types

## DCore Config
This config class stores information *specific to the functioning of the library itself.* The settings are not saved to any files. 

## Global Bot Config
This config class stores information *not related to the functioning of the library, but that apply to all bots.* It can be manually modified as it's saved as a JSON file.
It's shared by all accounts, so it's useful for creating universal configuration settings. It can be accessed via ConfigManager in BotManager.

## Bot Config
This config class stores information *relating to a specific bot account.* It's saved as a JSON file for each bot account.
This class will be the most useful for most applications.


# Config Extensions
The included settings in GlobalBotConfig and BotConfig classes only include general settings, likely to be used in a standard bot. However most bots have special requirements, not covered by the included settings. 
You can still store those settings using DCore's config system by using Extensions. Their type has to be specified when creating the ConfigManager (Global Config) and using BotManager.RequestBots (Bot Config).

```cs
var bots = botManager.RequestBots(1, typeof(ExampleBotConfigExtension));
var botConfig = bots.First().Config;
string example = (botConfig.Extension as ExampleBotConfigExtension).ExampleCustomVariable;
```

These can be any type, and saving/loading those will be handled automatically. You can access them via the Extension property in the respective configuration classes, but you need to cast them.
