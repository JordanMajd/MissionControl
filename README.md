# Mission Control 🚀

Mission Control is set of modding tools and utilities for [Mars: First Logistics Demo](https://store.steampowered.com/app/1532200/Mars_First_Logistics/), built for the community and by the community.

⚠ This softwave is experimental ALPHA, we are working towards a stable API but we are nowhere near. But right now we are just thorwing things at walls and seeing what sticks. Structure will come with time. Expect breaking changes.

## Getting Started

TODO: real documenation

1. Install [BepinEx v6 IL2CPP](https://builds.bepinex.dev/projects/bepinex_be)
1. Install [UniverseLib IL2CPP](https://github.com/sinai-dev/UniverseLib)
1. Build this running `dotnet build` then copy the dll into the bepinx plugin directory. This can be be done by running `./build`

Optional steps:

- Copy `Examples/examplebundle`, into your gamedata folder to test custom meshes and parts. This can be done manually or by running `./copy_examples.ps1`)
- If you are developing your own mod I highly suggest install [UnityExplorer, IL2CPP version](https://github.com/sinai-dev/UnityExplorer)

### Features

- Asset Utils: Tools to load assets, textures, meshes
- Part Manager: Tools to import custom parts with their own meshes
  - Limitations:
    - Can import `PartSO` and `WheelSO`, other types to follow
    - Localization is broken
    - Custom meshes don't have textures / materials
    - No custom part icons yet
- Cheat Manager:
  - Disable Gravity
  - Give 9999 parts (Enabled by default for testing atm)
  - Unlock all parts (Enabled by default for testing atm)

### Future Work

- Cheat Manager
- Contract Manager
- Parcel Manger
- Station Manager

## Contributing

TODO

## Changelog

TODO

## License 

- MIT, see [LICENSE](/LICENSE) for more information.