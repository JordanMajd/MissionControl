# Modding Mars First Logistics Demo

⚠ Do at your own risk. It is recommended to back up the game before modifying ⚠

These instructions are for Windows, however this works for all platforms, you'll just need to download the appropriate dll for your platform instead.

## Installing Bepinex

In order to install any mods, we must first install Bepinex:

1. Download the latest BepInEx Unity (**IL2CPP version not mono**) for Windows (x64) from [bepninex builds](https://builds.bepinex.dev/projects/bepinex_be) (under the artifacts section)
  - File name should have `IL2CPP` in it and look similar to this: `BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.660+40bf261.zip`
1. Locate your `Game Root` folder. To do this open Steam and navigate to Library -> Mars First Logistics Demo -> Right Click -> Properties -> Game Files -> Browse
1. Follow these instructions to [install Bepinex Il2CPP](https://docs.bepinex.dev/master/articles/user_guide/installation/unity_il2cpp.html):
  - Extract the contents of the download into the `Game Root` folder
  - From the `Game Root` folder, run the game once by clicking on `Mars First Logistics.exe` to generate the bepinex files
  - After the game runs you may close it
1. From now on when you launch the game, launch it from "Steam" instead of clicking on the exe. 
1. You are now ready to install any mods you want :)

## Installing Mods

Here are instructions on how to install mods

### Setup Mission Control

TODO: How to install [Mission Control](https://github.com/JordanMajd/MissionControl)
- TODO publish release
- If you are comfortable running and compiling code, see [README.md](/README.md) for instructions building the mod yourself

### Setup Unity Explorer

If you are developing mods or want to learn how the internals of the game work and tweak them yourself I recomend installing Unity Explorer

1. Install [UnityExplorer, IL2CPP CoreCLR version](https://github.com/sinai-dev/UnityExplorer)
  - `You'll want to download **BepInEx BIE 6.X IL2CPP (CoreCLR)**, linked in the [here in the bepinex section of the README](https://github.com/sinai-dev/UnityExplorer#bepinex)
  - Unzip the release file into a folder
  - Take the whole `plugins/sinai-dev-UnityExplorer` folder and place it in the games root `BepInEx/plugins/`
  - So the final folder structure should look like `<game root path>/BepinEx/plugins/sinai-dev-UnityExplorer`
  - Open Mars First Logistics via Steam

If This gives you trouble you can instead download [this precompiled version of UnityExplorer / UniverseLib](https://locoserver.net/dl/unityexplorer_bie6.zip). [More details in this github issue.](https://github.com/sinai-dev/UnityExplorer/issues/169#issuecomment-1251730571])

## Further Resources

Some people have had success using [this guide](https://framedsc.com/GeneralGuides/universal_unity_freecam.htm) for installing universe lib

- [Installing Bepinex](https://docs.bepinex.dev/master/articles/user_guide/installation/index.html)
