# AntiAfkKick
An application, Dalamud and ACT plugin for preventing being auto-kicked from FFXIV due to inactivity.

## Dalamud plugin
Recommended if you are using FFXIV Quick Launcher. 

Add my custom repo URL: `https://raw.githubusercontent.com/Eternita-S/MyDalamudPlugins/main/pluginmaster.json` then install plugin from plugins list.

Detailed instruction available here: https://github.com/Eternita-S/MyDalamudPlugins

## ACT plugin
Download it here: https://github.com/Eternita-S/AntiAfkKick/releases/download/1.0.0.0/AntiAfkKick-ACT.dll

Then copy it to any convenient folder. Open up ACT, go to "Plugins" tab, then to "Plugin Listing", then click "Browse" button and select DLL file you have just downloaded, and finally click "Add/Enable plugin". Upon doing so ACT may ask you to unblock file. In this case press "Yes".


## Standalone version
Just download it and run it. No configuration needed. Only supports one instance of FFXIV. To exit, access program's tray icon.

Download link: https://github.com/Eternita-S/AntiAfkKick/releases/download/1.0.0.0/AntiAfkKick.exe

## How it works?
It just sends left control key to the game every now and then. Seems like for now it's enough to make game think user is still active. It won't send keypress if you are actually playing.
