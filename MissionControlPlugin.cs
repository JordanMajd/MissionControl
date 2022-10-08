
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;

using HarmonyLib;

using System.Threading.Tasks;
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

    // late mount components
    AddDelayedComponents();
  }
  async void AddDelayedComponents()
  {
    await Task.Delay(5000);
    LocalizationManager lm = AddComponent<LocalizationManager>();
    CheatManager cm = AddComponent<CheatManager>();
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

