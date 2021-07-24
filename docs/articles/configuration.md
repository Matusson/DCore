This guide goes more in-depth on how to use the configuration functions in DCore.

# Config Types

## DCore Config
This config class stores information *specific to the functioning of the library itself.* The settings are not saved to any files. 
Specific settings are documented in the API portion of this documentation (and via Intellisense).

## Global Bot Config
This config class stores information *not related to the functioning of the library, but that apply to all bots.* It can be manually modified as it's saved as a JSON file.
It's shared by all accounts, so it's useful for creating universal configuration settings. It can be accessed via ConfigManager in BotManager.

## Bot Config
This config class stores information *relating to a specific bot account.* It's saved as a JSON file for each bot account.
This class will be the most useful for most applications.