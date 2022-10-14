# Extracting Assets / Meshes

The easiest way to extract assets from the game is via [AssetStudio](https://github.com/Perfare/AssetStudio) 

1. Open AssetStudio
2. Select File → Load File
3. Select `sharedassets0.assets` from your game's data folder
  - Find your games data folder by opening Steam and navigating to Library → Mars First Logistics Demo → Right Click → Properties → Game Files → Browse then navigating to `Mars First Logistics_Data`
  - Example data folder: `C:\Program Files (x86)\Steam\steamapps\common\Mars First Logistics Demo\Mars First Logistics_Data\`
4. Select `Asset List` Tab, you can sort by type to make it easier to find the item you are looking for.
-  If you are looking for a mesh, chances are it will be called `default`, you'll need to use the preview on the righthand side to find the exact one you are looking for.
6. Once you locate the asset you are looking for right click it and select `Export Selected Assets`
7. Select the folder you want to export it and press save/export
8. Now within the folder you exported, you will have a folder called `Mesh`, within that folder should be the exported asset `default.obj`

## Other tools

- [AssetRipper](https://github.com/AssetRipper/AssetRipper) is another good option. This is more if you are looking to import the assets into Unity