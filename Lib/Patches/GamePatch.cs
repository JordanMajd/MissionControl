using HarmonyLib;
using UnityEngine;

namespace MissionControl.Patches;

// Patch Game Instance
// This is only here to serve as example
public class GamePatch: MonoBehaviour
{
  public void Awake()
  {
    MissionControlPlugin.Harmony.PatchAll(typeof(Patches));
  }

  private class Patches
  {

    [HarmonyPatch(typeof(Game))]
    [HarmonyPatch(nameof(Game.Init))]
    static void Postfix(Game __instance)
    {
      try
      {
        MissionControlPlugin.Log.LogInfo("Game initialized!");
      }
      catch (System.Exception ex)
      {
        MissionControlPlugin.Log.LogWarning($"Exception in patch of void Game::Init():\n{ex}");
      }
    }
  }

}


