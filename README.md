# Become a Supporter!
If you like this or any of the other plugins I maintain, please consider becoming a supporter on Patreon or via other means! This will help me to continue updating the plugin and implement new features and you will receive benefits such as priority support, prioritized feature request, early testing builds and private plugins as well - number of which will only grow with Dawntrail. 
- [Subscribe on Patreon](https://subscribe.nightmarexiv.com/) - eligible for Discord role
- [Donate Litecoin, Bitcoin, Tether or other crypto](https://crypto.nightmarexiv.com/) - eligible for Discord role
- [One-time donation via Ko-Fi](https://donate.nightmarexiv.com/)

### Also:
- [Explore other plugins I maintain or contributed to](https://explore.nightmarexiv.com/)
- [Join NightmareXIV Discord server to receive fast support and pings about plugin updates](https://discord.gg/m8NRt4X8Gf)
# AntiAfkKick

|Until Dalamud framework is available again, please use standalone version. [Click here to download](https://github.com/NightmareXIV/AntiAfkKick/releases/download/2.1.0.4/AntiAfkKick-Standalone.exe).|
|---|

|Attention! Standalone version sometimes may have an issue if you're using a controller and afk without alt-tabbing out of the game. If you experience this issue, alt-tab before going AFK.|
|---|

An application, Dalamud and ACT plugin for preventing being auto-kicked from FFXIV due to inactivity.

**Please use responsibly, this is meant to be used for gatherers or if you are waiting for raid/pf/event, not to just afk in limsa dancing**

## Dalamud plugin
Recommended if you are using FFXIV Quick Launcher. Using AntiAfkKick as Dalamud plugin provides advantages of automatically launching together with game and automatic updates. ACT plugin and standalone version must be updated manually if ever needed.

Add my custom repo URL: 

`https://raw.githubusercontent.com/NightmareXIV/MyDalamudPlugins/main/pluginmaster.json` 

then install plugin from plugins list.

Detailed instruction available here: https://github.com/NightmareXIV/MyDalamudPlugins

## ACT plugin
Download it here: https://github.com/NightmareXIV/AntiAfkKick/releases

Then copy it to any convenient folder. Open up ACT, go to "Plugins" tab, then to "Plugin Listing", then click "Browse" button and select DLL file you have just downloaded, and finally click "Add/Enable plugin". Upon doing so ACT may ask you to unblock file. In this case press "Yes".


## Standalone version
Just download it and run it. No configuration needed. To exit, access program's tray icon.

Download link: https://github.com/NightmareXIV/AntiAfkKick/releases/download/2.1.0.4/AntiAfkKick-Standalone.exe

## How it works?
It just sends left control key to the game every now and then. Seems like for now it's enough to make game think user is still active. It won't send keypress if you are actually playing.

