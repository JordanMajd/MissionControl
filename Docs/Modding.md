# Modding Mars First Logistics Demo

⚠ Do at your own risk. It is recommended to back up the game before modifying. ⚠

These instructions are for Windows x64, however the same procedure works for all platforms by using the appropriate dll.

## Installing BepInEx

In order to install any mods, we must first install Bepinex:

1. If you don't already have a .NET 6.0 SDK installed, navigate to [latest .NET releases](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) to download and run the installer for your platform.
1. Download [BepInEx Bleeding Edge #656](https://builds.bepinex.dev/projects/bepinex_be) ([Windows x64 download](https://builds.bepinex.dev/projects/bepinex_be/656/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.656%2Bb3485f4.zip)).
    - It is important to ensure this is the version downloaded, as it ensures [UnityExplorer](https://github.com/sinai-dev/UnityExplorer) compatibility.
    - If you are installing for another platform you can find [the correct release here](https://builds.bepinex.dev/projects/bepinex_be). Please ensure you download version `#656`. The file name should have `IL2CPP` in it and look similar to this: `BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.656+XXXXXXX.zip`, where the `X` represent alphanumeric characters and `win/x64` are replaced by your platform.
    - Do not use the [latest GitHub release](https://github.com/BepInEx/BepInEx/releases) as it will not work due to the use of .NET 6.0.
1. Locate your `<GameRoot>`. 
    - You can do this by opening Steam and navigating to Library → Mars First Logistics Demo → Right Click → Properties → Game Files → Browse.
1. Extract the contents of the download into the `<GameRoot>` folder.
1. Run the game either from Steam or from the `<GameRoot>` folder by clicking on `Mars First Logistics.exe` to generate the BepInEx files.
1. After the game runs you may close it.
1. You are now ready to install any mods you want!
    - If you are looking to install Mission Control, continue reading [Adding UniverseLib](#adding-universelib).

### Adding UniverseLib

[UniverseLib](https://github.com/sinai-dev/UniverseLib) is a required library with a focus on UI which Mission Control makes use of for some of its features. 

ℹ️ You can also [setup UnityExplorer](#setup-unityexplorer), which is a mod that has this library bundled with it. ℹ️

1. Download this [this precompiled version of UnityExplorer](https://locoserver.net/dl/unityexplorer_bie6.zip).
    - Ideally the [latest UniverseLib IL2CPP Interop version](https://github.com/sinai-dev/UniverseLib/releases/latest/download/UniverseLib.Il2Cpp.Interop.zip) would be used, but [issues with .NET 6.0](https://github.com/sinai-dev/UnityExplorer/issues/169#issuecomment-1251730571]) force the use of this precompiled version.
1. Unzip the contents of the download.
1. Navigate into `plugins/sinai-dev-UnityExplorer` and copy the `UniverseLib.IL2CPP.Interop.dll` file to `<GameRoot>/BepInEx/plugins/`.
    - The final file structure should look like: `<GameRoot>/BepinEx/plugins/UniverseLib.IL2CPP.Interop.dll`

## Installing Mods

Mods are, as the name suggests, modifications to the game. Here is how to install them.

### Setup Mission Control

[Mission Control](https://github.com/JordanMajd/MissionControl) is a plugin and library that adds a series of [features](/README.md#features) to Mars First Logistics.

Before proceeding, please ensure you have completed these prerequesite steps:
1. [Installing BepInEx](#installing-bepinex)
2. [Adding UniverseLib](#adding-universelib)

Once the prerequisites have been completed you are ready to install Mission Control:
1. Download the latest .dll (example: `MissionControl-0.0.1-alpha.dll`) from [Releases](https://github.com/JordanMajd/MissionControl/releases).
1. Create the folder `MissionControl` in `<GameRoot>/BepinEx/plugins/` if it doesn't exist.
1. Copy the downloaded file to `<GameRoot>/BepinEx/plugins/MissionControl`.
    - The final folder structure should look like: `<GameRoot>/BepinEx/plugins/MissionControl/MissionControl-X.dll`, where `X` is the version.
1. Mission Control is now installed!

Now that the mod is installed, you can add custom parts. To use the examples available in this repository:
1. Download all assets in the [Examples](https://github.com/JordanMajd/MissionControl/tree/master/Examples) folder (you can do so automatically [here](https://download-directory.github.io/?url=https%3A%2F%2Fgithub.com%2FJordanMajd%2FMissionControl%2Ftree%2Fmaster%2FExamples)).
1. Copy them all to `<GameRoot>/Mars First Logistics_Data/ModResources`.
1. The parts are now installed!
    - You will need to use the "Unlock all parts" cheat in order to see them in the "Edit Vehicle" scene.

ℹ️ If you are interested in building and running the project manually please see [Compiling on your own](/README.md#Compiling-on-your-own). ℹ️ 

### Setup UnityExplorer

[UnityExplorer](https://github.com/sinai-dev/UnityExplorer) is a tool that allows players to explore and modify Unity games, a very handy plugin for those wanting to play around and learn the internals of Mars First Logistics.

1. Download this [this precompiled version of UnityExplorer](https://locoserver.net/dl/unityexplorer_bie6.zip).
    - Ideally the [latest UnityExplorer IL2CPP CoreCLR version](https://github.com/sinai-dev/UnityExplorer/releases/latest/download/UnityExplorer.BepInEx.IL2CPP.CoreCLR.zip) would be used, but [issues with .NET 6.0](https://github.com/sinai-dev/UnityExplorer/issues/169#issuecomment-1251730571]) force the use of this precompiled version.
1. Unzip the contents of the download.
1. Copy the `plugins/sinai-dev-UnityExplorer` folder and place it in `<GameRoot>/BepInEx/plugins/`.
    - The final folder structure should look like: `<GameRoot>/BepinEx/plugins/sinai-dev-UnityExplorer`
1. UnityExplorer is installed!

## Concepts

- `<GameRoot>`: the installation path for the game
- "plugin": mainly used as a synonym for "mod"
