# Mission Control 🚀

Mission Control is set of modding tools and utilities for [Mars First Logistics Demo](https://store.steampowered.com/app/1532200/Mars_First_Logistics/), built for the community and by the community. It can be used as a mod by itself or it can be used as a library to help create mods.

⚠ This softwave is experimental ALPHA, we are working towards a stable API but we are nowhere near. But right now we are just thorwing things at walls and seeing what sticks. Structure will come with time. Expect breaking changes.

## Features

Current / In Progress:
- Part Manager: Import part packs and custom meshes
    - Custom meshes don't have materials
    - No custom part icons yet
- Cheat Manager: In game menu can be opened by pressing `F8`
  - Disable / Enable Vehicle Gravity
  - Give 999,999 funds
  - Give 9999 parts
  - Unlock all parts
  - Set max speed 99999
  - Remove build limit
- Asset Utils: Tools to load assets, textures, meshes
- Localization Manager: load & manage localization strings for custom parts

Future:
- Contract Manager
- Parcel Manger
- Station Manager

## Getting Started

### Applying the mod

There are two main requirements:
- [BepInEx](https://github.com/BepInEx/BepInEx/releases), a tool that enables modding in Unity games
- [UniverseLib](https://github.com/sinai-dev/UniverseLib), or in its defect [UnityExplorer](https://github.com/sinai-dev/UnityExplorer), which bundles the prior

For more details, please see the [Modding](/Docs/Modding.md) documentation.

### Compiling on your own

Since the project uses your game installation as the source for libraries, the same requirements as in the previous section are in place. Refer to [Installing BepInEx](/Docs/Modding.md#installing-bepinex) and [Adding UniverseLib](/Docs/Modding.md#adding-universelib) for further documentation. 

After the required installation is completed:
1. Build and copy the dll to your game by running `dotnet build /t:Deploy`
1. Copy an example parts pack to your game by running `dotnet build /t:Examples`
1. Create an `Env.props` file if your game installation path is not the default Steam path. The file should look as follows:

```
<Project>
  <PropertyGroup>
    <GAME_PATH>C:\Program Files (x86)\Steam\steamapps\common\Mars First Logistics Demo</GAME_PATH>
  </PropertyGroup>
</Project>
```

## Contributing

We welcome your contributions!

Have a suggestion or want to report a bug?
1. Search [issues](https://github.com/JordanMajd/MissionControl/issues) to see if the suggestion or bug has already been reports
1. If it hasn't, open up a [new issue](https://github.com/JordanMajd/MissionControl/issues/new) and provide as detailed of an explanation as you can

Want to help write some code but not sure where to start? 
- Take a look at our [open issues](https://github.com/JordanMajd/MissionControl/issues) and look for ones that have been triaged and tagged

## License 

- MIT, see [LICENSE](/LICENSE) for more information
