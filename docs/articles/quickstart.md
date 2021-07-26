# Creating Bot Manager
The first step in using DCore is creating a BotManager. The BotManager, as name implies, is a class that manages loading account information, distributing it, and also contains various utility functions if you need to use multiple accounts in your software.
The recommended way to do it is:
```cs
BotManager manager = new BotManager(x => {
	x.UseMultipleBots = false;
    });
```
You can use the lambda expression to assign config values. This also allows you to specify the config extension Type, which will be described in a separate guide.
Note that this approach does not use Dependency Injection. This is because if you wish to use DI, you will probably also want DiscordSocketClient in your DI Container, which will be created later.

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

# Requesting accounts
The last thing you will need to do is request the bot accounts from the bot manager:
```cs
int requestedBotCount = 1;
var activatedBots = botManager.RequestBots(requestedBotCount);
```
This will take the selected number of unused accounts and return them as objects. 

> [!NOTE]
> By default, bots will automatically start up after being requested. You can change this behaviour via DCore config. 

# Dependency Injection
If you wish to use Dependency Injection, you can register DCore services with BotManager.AddDCoreServices(IServiceCollection):
```cs
botManager.AddDCoreServices(serviceContainer);
```
It's recommended that you do this after requesting bots. This will always add DCoreConfig and the BotManager, but if a bot has been requested, this will also add DiscordBot and its services (DiscordSocketClient, DCoreLogger, BotConfig, LanguageManager) to the DI container.
Also note that if you set UseMultipleBots to true in DCoreConfig, this will not add the bots or their services to the DI container.

# Fetching bots from BotManager
BotManager.RequestBots() will return a list of bots activated  by that action. Once bots have been activated via RequestBots(), there are ways to fetch these bots from BotManager. 
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