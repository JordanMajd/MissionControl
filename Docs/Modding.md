# Modding Mars First Logistics Demo

⚠ Do at your own risk. It is recommended to back up the game before modifying. ⚠

These instructions are for Windows x64, however the same procedure works for all platforms by using the appropriate dll.

## Installing BepInEx

In order to install any mods, we must first install Bepinex:

1. If you don't already have a .Net 6 SDK installed, navigate to [Latest .Net Releases](https://dotnet.microsoft.com/en-us/download) to download and run the installer for your platform
1. Download BepinEX Edge #656
    - [Windows x64 Download](https://builds.bepinex.dev/projects/bepinex_be/656/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.656%2Bb3485f4.zip)
    - If you are installing for another platform you can find [the correct release here](https://builds.bepinex.dev/projects/bepinex_be). Please ensure you download version `656`, the file name should have `IL2CPP` in it and look similar to this: `BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.656+XXXXXXX.zip`, where the `X` represent alphanumeric characters and `win/x64` are replaced by your platform.
    - Do not use the [latest GitHub release](https://github.com/BepInEx/BepInEx/releases) as it will not work due to the use of NET 6.0
1. Locate your `<GameRoot>`. 
    - You can do this by opening Steam and navigating to Library → Mars First Logistics Demo → Right Click → Properties → Game Files → Browse
1. Extract the contents of the download into the `<GameRoot>` folder
1. Run the game either from Steam or from the `<GameRoot>` folder by clicking on `Mars First Logistics.exe` to generate the BepInEx files
1. After the game runs you may close it
1. You are now ready to install any mods you want!
    - If you are looking to install Mission Control, continue reading [Adding UniverseLib](#adding-universelib)

### Adding UniverseLib

UniverseLib is a required library with a focus on UI which Mission Control makes use of for some of its features. 

ℹ️ You can also [setup UnityExplorer](#setup-unityexplorer), which is a mod that has this library bundled in it. ℹ️

1. Download this [this precompiled version of UnityExplorer](https://locoserver.net/dl/unityexplorer_bie6.zip)
    - Ideally the [UniverseLib IL2CPP Interop version](https://github.com/sinai-dev/UniverseLib) would be used, but [issues with NET 6.0](https://github.com/sinai-dev/UnityExplorer/issues/169#issuecomment-1251730571]) force the use of this precompiled version
1. Unzip the contents of the download
1. Navigate into `plugins/sinai-dev-UnityExplorer` and copy the `UniverseLib.IL2CPP.Interop.dll` file to `<GameRoot>/BepInEx/plugins/`
    - The final file structure should look like `<GameRoot>/BepinEx/plugins/UniverseLib.IL2CPP.Interop.dll`

## Installing Mods

Mods are, as the name suggests, modifications to the game. Here is how to install them.

### Setup Mission Control

How to install [Mission Control](https://github.com/JordanMajd/MissionControl)

Please ensure you have completed the prerequesite steps:
1. [Installing BepInEx](#installing-bepinex)
2. [Adding UniverseLib](#adding-universelib)

Once the prerequisites have been completed you are ready to install Mission Contorl:
1. Download the latest .dll (example: `MissionControl-0.0.1-alpha.dll`) from [Releases](https://github.com/JordanMajd/MissionControl/releases)
1. Copy it to `<GameRoot>/BepinEx/plugins/MissionControl/`

ℹ️ If you are interested in building and running the project manually please see [Compiling on your own](/README.md#Compiling-on-your-own) ℹ️ 

### Setup UnityExplorer

UnityExplorer is a tool that allows players to explore and modify Unity games, a very handy plugin for those wanting to play around and learn the internals of Mars First Logistics.

⚠ UnityExplorer does not offer full functionality due to the use of NET 6.0. ⚠

1. Download this [this precompiled version of UnityExplorer](https://locoserver.net/dl/unityexplorer_bie6.zip)
    - Ideally the [UnityExplorer IL2CPP CoreCLR version](https://github.com/sinai-dev/UnityExplorer) would be used, but [issues with NET 6.0](https://github.com/sinai-dev/UnityExplorer/issues/169#issuecomment-1251730571]) force the use of this precompiled version
1. Unzip the contents of the download
1. Copy the `plugins/sinai-dev-UnityExplorer` folder and place it in `<GameRoot>/BepInEx/plugins/`
    - The final folder structure should look like `<GameRoot>/BepinEx/plugins/sinai-dev-UnityExplorer`
1. UnityExplorer is installed!

## Concepts

- `<GameRoot>`: the installation path for the game
- "plugin": mainly used as a synonym for "mod"
