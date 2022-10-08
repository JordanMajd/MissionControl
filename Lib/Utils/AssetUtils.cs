
using System.Collections.Generic;
using System.IO;
using SimpleJSON;

namespace MissionControl.Utils;


static class Assets
{
  private static string ModResourcesPath = Path.Combine(BepInEx.Paths.GameDataPath, "ModResources");

  // UniverseLib.AssetBundle.GetAllLoadedAssetBundles was giving me some trouble loading too slowly
  // so we're just going to load things as quickly as possible using our own cache
  // TODO make sure we clean up these asset bundles so we aren't taking up an insane amount of memory
  public static Dictionary<string, UniverseLib.AssetBundle> cachedBundles = new Dictionary<string, UniverseLib.AssetBundle>();
  public static T LoadAsset<T>(string bundleName, string assetName) where T : UnityEngine.Object
  {
    UniverseLib.AssetBundle assetBundle;
    if (!cachedBundles.TryGetValue(bundleName, out assetBundle))
    {
      string path = Path.Combine(ModResourcesPath, bundleName);

      if (!File.Exists(path))
      {
        MissionControlPlugin.Log.LogError($"LoadAsset, file {path} does not exists");
        return null;
      }
      assetBundle = UniverseLib.AssetBundle.LoadFromFile(path);
      cachedBundles.Add(bundleName, assetBundle);
    }
    return assetBundle.LoadAsset<T>(assetName);
  }


  public static JSONNode LoadJSON(string jsonName)
  {
    string path = Path.Combine(ModResourcesPath, jsonName);

    if (!File.Exists(path))
    {
      MissionControlPlugin.Log.LogError($"LoadJSON, file {path} does not exists");
      return null;
    }

    string jsonText = File.ReadAllText(path);

    return JSON.Parse(jsonText);
  }


  public static List<string> GetJSONFiles()
  {
    List<string> assetPacks = new List<string>();
    if (Directory.Exists(ModResourcesPath))
    {
      string[] filesNames = Directory.GetFiles(ModResourcesPath);
      foreach (string fileName in filesNames)
      {
        if (fileName.EndsWith(".json"))
        {
          assetPacks.Add(fileName);
        }
      }
    }

    return assetPacks;
  }
}
