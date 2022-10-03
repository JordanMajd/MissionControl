
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

using HarmonyLib;

using System.Threading.Tasks;

namespace MissionControl;
using MissionControl.Patches;


[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class MissionControlPlugin : BasePlugin
{

  internal static new ManualLogSource Log;
  internal static Harmony Harmony;

  public override void Load()
  {
    // set up logger
    MissionControlPlugin.Log = base.Log;
    MissionControlPlugin.Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

    // set up harmony patcher
    Harmony = new Harmony("com.missioncontrol.patch");
  
    MissionControlPlugin.Log.LogInfo("All patches applied");
    GamePatch gp = AddComponent<GamePatch>();
    PartsManager pm = AddComponent<PartsManager>();

    // late mount components
    AddDelayedComponents();
  }
  async void AddDelayedComponents()
  {
    await Task.Delay(3000);
    // MissionControlComponent sc = AddComponent<MissionControlComponent>();
    // MissionControlPlugin.Log.LogInfo($"Plugin mounted");
  }
}



