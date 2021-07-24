# Creating Bot Manager
The first step in using DCore is creating a BotManager. The BotManager, as name implies, is a class that manages loading account information, distributing it, and also contains various utility functions if you need to use multiple accounts in your program.
If you're using Dependency Injection, creating one is trivial:
```cs
collection = new ServiceCollection();
collection.AddDCore(x => {
	x.UseMultipleBots = true;
    });
```
This will add all required services into your DI container. You can use the lambda expression to assign config values. This also allows you to specify the config extension Type, which will be described in a separate guide.

That is the recommended way, but in case you can't use DI, you have to manually create the services:
 ```cs
DCoreConfig config = new();
ConfigManager configManager = new(config);
BotManager manager = new(configManager, config);
```
In this case, make sure that you only have one instance of each in your code.

# Loading bot information
To manage any bots with BotManager, you first need to load token information into it. There are multiple ways to do it.
For most use cases, you should only use one bot account. In that case, you can simply use:
```cs
ulong botId = 12345678;
string botToken = "VALIDTOKEN";
botManager.LoadAccount(botId, botToken);
```

In case you need multiple accounts, the simplest way is to store that information in a file, in which every line consists of the bot ID and a token, separated by a space. You can load that information like this:
```cs
string path = "accounts.txt";
botManager.LoadAccountsFromFile(path);
```

Alternatively, if you wish to load the accounts in another way, you can manually create and load a list of TokenInfo structs:
```cs
//Add any account information you need to this list
List<TokenInfo> accounts = new();
botManager.LoadAccounts(accounts);
```

# Activating accounts
The last thing you will need to do is request the bot accounts from the bot manager:
```cs
int requestedBotCount = 1;
var activatedBots = botManager.RequestBots(requestedBotCount);
```
This will take the selected number of unused accounts and return them as objects. 

> [!NOTE]
> Please note that by default, this will not mean that the bots will log into Discord. This process just creates the DiscordBot objects for you to use. 