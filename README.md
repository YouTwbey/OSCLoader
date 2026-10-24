<img width="6480" height="2160" alt="oscloader-2160-full" src="https://github.com/user-attachments/assets/3c96db39-b9f3-40b0-8307-747705384766" />

DISCLAIMER: THIS IS NOT A CHEAT CLIENT AND DOES NOT TAMPER WITH VRCHAT IN ANY WAY. THIS IS ONLY AN OSC APP.

An OSC app for VRChat that has the appearance of MelonLoader while allowing for running C# code externally from the game.

THIS APP IS STILL IN DEVELOPMENT, FEEL FREE TO [JOIN THE DISCORD](discord.gg/jTtw7PRgHu) TO KEEP UP TO DATE!

# Features
## External C# DLL Loading
Instead of OSC features being built in, OSCLoader works in a very similar way as a mod loader, allowing you to write your own DLLs to perform OSC actions and more!
## Standalone Quest Support
OSCLoader works completely on Standalone! The only time a PC will or may be required is to install the APK and to add or remove mods.
## Data Reading
OSCLoader gives more data than other OSC apps. It allows you to read data such as avatar data, VRChat settings, Photon Networking data, and more!
## Log Viewer
OSCLoader opens with a console to allow you to view logs coming from any mods loaded or anything VRChat or any worlds log.
## Discord RPC Support
OSCLoader overwrites VRChat's default Discord's RPC to allow for a more appealing game status, such as showcasing info about the world you are in or even current in-game events depending on the world.
## World Support
OSCLoader has an UDON package to allow for World Support. This will only work if the world creator willingly adds OSCLoader support to their world itself. If so, this will allow you to interact with VRChat in many new ways!

# Setup
## PC
1. Download the OSCLoader-VERSION.zip file.
2. Extract the files located in "PC Version".
3. Move the extracted files to VRChat's install location.
4. If it asks to replace any files, replace them. This is safe to do so.
5. Open the game and make sure you have all logs turned on. This will expand OSCLoader's features and allow it to run at it's best.
## Quest
1. Download the OSCLoader-VERSION.zip file.
2. Extract the APK located in "Quest Version".
3. Install it to your Quest device using something like Sidequest.
6. Open OSCLoader through Unknown Sources.
7. Give OSCLoader any access it needs.
8. Open VRChat through OSCLoader and make sure you have all logs turned on. This will expand OSCLoader's features and allow it to run at it's best.
## UDON World Support
1. Download the OSCLoader-VERSION.zip file.
2. Extract the C# Script located in "UDON World Support".
3. Create a U# Script in your VRChat Project and call it "OSCLoader".
4. Copy and paste the code from the C# script extracted into your newly created U# script.
5. Attach the script to a GameObject in the scene.
6. You can now refer to this U# script anywhere in your game to make OSCLoader calls!
