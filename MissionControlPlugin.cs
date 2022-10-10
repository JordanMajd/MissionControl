using UnityEngine;

using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;

using HarmonyLib;

namespace MissionControl;
using MissionControl.Patches;


[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class MissionControlPlugin : BasePlugin
{

  internal static new ManualLogSource Log;
  internal static Harmony Harmony;
  internal static MCConf MCConf;

  public override void Load()
  {
    // set up logger
    MissionControlPlugin.Log = base.Log;
    MissionControlPlugin.Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

    // set up harmony patcher
    Harmony = new Harmony("com.missioncontrol.patch");
    MissionControlPlugin.Log.LogInfo("All patches applied");

    // set up config 
    MCConf = new MCConf(Config);

    GamePatch gp = AddComponent<GamePatch>();

    PartsManager pm = AddComponent<PartsManager>();

    MissionControlPlugin.Log.LogInfo("UniverseLib start");

    // default config for UniverseLib (https://github.com/sinai-dev/UniverseLib/wiki/Initialization)
    float startupDelay = 1f;
    UniverseLib.Config.UniverseLibConfig config = new()
    {
        Disable_EventSystem_Override = false, // or null
        Force_Unlock_Mouse = true, // or null
        Unhollowed_Modules_Folder = System.IO.Path.Combine(BepInEx.Paths.BepInExRootPath, "interop") // or null
    };

    // init UniverseLib and do late mount components
    UniverseLib.Universe.Init(startupDelay, AddDelayedComponents, LogHandler, config);
  }
  void AddDelayedComponents()
  {
    MissionControlPlugin.Log.LogInfo("Adding late mounts");
    LocalizationManager lm = AddComponent<LocalizationManager>();
    CheatManager cm = AddComponent<CheatManager>();
  }

  void LogHandler(string message, LogType type) 
  {
    MissionControlPlugin.Log.LogInfo(message);
  }
}

class MCConf
{

  public ConfigEntry<bool> autoLoadAssetPacks;
  public MCConf(ConfigFile Config)
  {
    // Part Manager
    string partManager = "PartManager";
    autoLoadAssetPacks = Config.Bind(partManager, "AutoLoadAssetPacks", true, "Automatically load asset packs in ModResources");
  }
}

