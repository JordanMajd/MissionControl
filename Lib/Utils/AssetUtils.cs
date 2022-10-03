using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using HarmonyLib;

using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

using Il2CppSystem;

namespace MissionControl.Utils;


static class Assets
{

  // UniverseLib.AssetBundle.GetAllLoadedAssetBundles was giving me some trouble loading too slowly
  // so we're just going to load things as quickly as possible using our own cache
  // TODO make sure we clean up these asset bundles so we aren't taking up an insane amount of memory
  public static Dictionary<string, UniverseLib.AssetBundle> cachedBundles = new Dictionary<string, UniverseLib.AssetBundle>();
  public static T LoadAsset<T>(string bundleName, string assetName) where T : UnityEngine.Object
  {
    MissionControlPlugin.Log.LogError($"Loading {bundleName}");

    UniverseLib.AssetBundle assetBundle;
    MissionControlPlugin.Log.LogError($"1");
    if (!cachedBundles.TryGetValue(bundleName, out assetBundle))
    {
      string path = Path.Combine(BepInEx.Paths.GameDataPath, bundleName);

      if (!File.Exists(path))
      {
        MissionControlPlugin.Log.LogError($"LoadAsset, file {path} does not exists");
        return null;
      }

      MissionControlPlugin.Log.LogError($"Loading {bundleName} from {path}");
      assetBundle = UniverseLib.AssetBundle.LoadFromFile(path);
      cachedBundles.Add(bundleName, assetBundle);
    }
    MissionControlPlugin.Log.LogError($"2");
    return assetBundle.LoadAsset<T>(assetName);
  }


  public static Il2CppSystem.Object LoadJSON(string jsonName, Type type)
  {
    string path = Path.Combine(BepInEx.Paths.GameDataPath, jsonName);

    if (!File.Exists(path))
    {
      MissionControlPlugin.Log.LogError($"LoadJSON, file {path} does not exists");
      return null;
    }

    string jsonText = File.ReadAllText(path);
    return UnityEngine.JsonUtility.FromJson(jsonText, type);
  }
}
