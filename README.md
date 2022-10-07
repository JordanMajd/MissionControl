# Mission Control 🚀

Mission Control is set of modding tools and utilities for [Mars: First Logistics Demo](https://store.steampowered.com/app/1532200/Mars_First_Logistics/), built for the community and by the community.

⚠ This softwave is experimental ALPHA, we are working towards a stable API but we are nowhere near. But right now we are just thorwing things at walls and seeing what sticks. Structure will come with time. Expect breaking changes.

## Getting Started

See the section on [Modding](/Docs/Modding.md) for more information on setting up BepinEX.

1. Install [BepinEx v6 IL2CPP](https://builds.bepinex.dev/projects/bepinex_be)
1. Install [UniverseLib IL2CPP](https://github.com/sinai-dev/UniverseLib)
1. You can build and copy the dll to your game by running `dotnet build /t:Deploy`

The project tries to find your game install, but if you need to configure it you can create `Env.props` file and provide it with a path to your game install:

```
<Project>
  <PropertyGroup>
    <GAME_PATH>C:\Program Files (x86)\Steam\steamapps\common\Mars First Logistics Demo</GAME_PATH>
  </PropertyGroup>
</Project>
```

### Features

- Part Manager: Import part packs and custom meshes
    - Localization is broken
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

### Future Work

- Contract Manager
- Parcel Manger
- Station Manager

## Contributing

TODO

## Changelog

TODO

## License 

- MIT, see [LICENSE](/LICENSE) for more information.