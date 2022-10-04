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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Game))]
    [HarmonyPatch(nameof(Game.Init))]
    static void Postfix_GameInit(Game __instance)
    {
      try
      {
        MissionControlPlugin.Log.LogInfo("Game initialized, all parts unlocked");
        __instance.partsInventory.UnlockAllParts();
        foreach (var entry in __instance.partsInventory.unlockedParts) {
          entry.Value.total = 9999;
        }
      }
      catch (System.Exception ex)
      {
        MissionControlPlugin.Log.LogWarning($"Exception in patch of void Game::Init():\n{ex}");
      }
    }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(Conf))]
    // [HarmonyPatch(nameof(Conf.Start))]
    // static void Postfix_ConfStart(Conf __instance)
    // {
    //   try
    //   {
    //     foreach(var part in __instance.partList.parts) {
    //       MeshRenderer mr = part.prefab.GetComponentInChildren<MeshRenderer>();
    //       Texture mainTex = mr.material.GetTexture("_MainTex");
    //       if(mainTex != null){
    //         MissionControlPlugin.Log.LogInfo($"texwidth: {mainTex.width}");
    //       }
    //     }
    //   }
    //   catch (System.Exception ex)
    //   {
    //     MissionControlPlugin.Log.LogWarning($"Exception in patch of void Conf::Init():\n{ex}");
    //   }
    // }

  }

}


