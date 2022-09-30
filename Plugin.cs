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

using System;

namespace HelloMars;


// XUnity.ResourceRedirector to redirect to other resources

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
  internal static new ManualLogSource Log;
  internal static Game Game;
  public override void Load()
  {
    Plugin.Log = base.Log;
    Plugin.Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

    Harmony harmony = new Harmony("com.hellomars.patch");
    harmony.PatchAll();
    Plugin.Log.LogInfo($"Patches applied");
    HelloComponent sc = AddComponent<HelloComponent>();
    // MountPlugin();
  }
  async void MountPlugin()
  {
    await Task.Delay(3000);
    HelloComponent sc = AddComponent<HelloComponent>();
    Plugin.Log.LogInfo($"Plugin mounted");
  }
}

public class HelloComponent : MonoBehaviour
{


  WheelSO bigWheel;
  PartSO.Action newAction;
  GameObject bigWheelPrefab;

  private UnityAction<Scene, LoadSceneMode> OnSceneLoaded = (UnityAction<Scene, LoadSceneMode>)((scene, mode) =>
  {
    Plugin.Log.LogInfo($"Scene {scene.name} is loaded!");
  });
  public void Awake()
  {
    Plugin.Log.LogInfo("Awake");
    gameObject.name = "Hello Mars";
    if (OnSceneLoaded != null)
    {
      SceneManager.add_sceneLoaded(OnSceneLoaded);
    }
  }
  void Start()
  {
    Plugin.Log.LogInfo("Start");
    GameObject gameConf = GameObject.Find("Conf");
    if (gameConf != null)
    {
      Conf confComp = gameConf.GetComponent<Conf>();

      if (confComp != null)
      {
        Plugin.Log.LogInfo("Creating BigWheel");

        Il2CppReferenceArray<PartSO> partsList = confComp.partList.parts;
        var wheelsList = partsList.OfType<WheelSO>();
        PartSO c = partsList.Where(part => part.name == "05Wheel1").First();
        WheelSO oldPart = c.Cast<WheelSO>();

        // PartsSO
        bigWheel = ScriptableObject.CreateInstance<WheelSO>();
        bigWheel.title = "Big Wheel";
        bigWheel.description = "Big Wheel";
        bigWheel.name = "100BigWheel";
        bigWheel.titleLocTerm = "BigWheel";
        bigWheel.titleTranslation = "Parts/BigWheel";
        bigWheel.cost = 600;
        bigWheel.mass = 32;
        bigWheel.includeInDemo = true;
        bigWheel.attachmentPoints = oldPart.attachmentPoints;
        bigWheel.meshGOPaths = oldPart.meshGOPaths;
        bigWheel.iconTex = oldPart.iconTex;
        bigWheel.order = oldPart.order + 100;
        bigWheel.ID = 100;

        // Actions
        // List<PartSO.Action> actions = new List<PartSO.Action>();
        // newAction = new PartSO.Action();
        // newAction.actionName = "Rocket Action";
        // newAction.locTerm = "Rocket Action Loc";
        // newAction.translation = "Parts/Bumper";
        // newAction.action = Control.Action.Rocket;
        // actions.Add(newAction);
        // newPart.actions = new(actions.ToArray());
        // newPart.SetActions();
        bigWheel.actions = oldPart.actions;

        // Prefab
        bigWheelPrefab =  GameObject.Instantiate(oldPart.prefab);
        bigWheelPrefab.name = "BigWheel";
        bigWheelPrefab.hideFlags = HideFlags.HideAndDontSave;
        bigWheelPrefab.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
        bigWheel.prefab = bigWheelPrefab;
        // bigWheel.prefab = oldPart.prefab;

        // Wheel parts
        bigWheel.motorForce = oldPart.motorForce * 4f;
        bigWheel.throttleSpeed = oldPart.throttleSpeed / 4f;
        bigWheel.sideFrictionCoef = oldPart.sideFrictionCoef;
        bigWheel.slipForceBoost = oldPart.slipForceBoost;
        bigWheel.stickyForce = oldPart.stickyForce;
        bigWheel.wheelTreadFreq = oldPart.wheelTreadFreq;
        bigWheel.wheelTreadOffset = oldPart.wheelTreadOffset;
        bigWheel.impactEventPath = oldPart.impactEventPath;
        bigWheel.wheelDamper = oldPart.wheelDamper;
        bigWheel.wheelSpring = oldPart.wheelSpring;
        bigWheel.applyForceRadius = oldPart.applyForceRadius * 4f;
        bigWheel.wheelWidth = oldPart.wheelWidth * 4f;
        bigWheel.wheelRadius = oldPart.wheelRadius * 4f;
        bigWheel.sideFrictionCurve = oldPart.sideFrictionCurve;

        // update config partslist
        List<PartSO> newPartsList = new List<PartSO>();
        foreach (var part in partsList)
        {
          newPartsList.Add(part);
        }
        newPartsList.Add(bigWheel);
        confComp.partList.parts = new(newPartsList.ToArray());
        // bigWheel.GetTranslations();
        bigWheel.AddToGlobalDict();

        if (Plugin.Game != null)
        {
          Plugin.Game.partsInventory.UnlockAllParts();
          Plugin.Log.LogInfo("All parts unlocked");
        }

      }
    }
  }
}

[HarmonyPatch(typeof(Game))]
[HarmonyPatch(nameof(Game.Init))]
class GamePatch
{
  static void Postfix(Game __instance)
  {
    try
    {

      Plugin.Game = __instance;
      Plugin.Log.LogInfo("Game instance assigned");
    }
    catch (System.Exception ex)
    {
      Plugin.Log.LogWarning($"Exception in patch of void Game::Init():\n{ex}");
    }
  }
}


[HarmonyPatch(typeof(Conf), MethodType.Constructor)]
class ConfPatch
{
  static void Postfix(Conf __instance)
  {
    try
    {

      Plugin.Log.LogInfo($"Conf constructed");

    }
    catch (System.Exception ex)
    {
      Plugin.Log.LogWarning($"Exception in patch of void Game::Init():\n{ex}");
    }
  }
}


// [HarmonyPatch(typeof(AssetBundle))]
// [HarmonyPatch(nameof(AssetBundle.Load))]
// [HarmonyPatch(new Type[] { typeof(string) })]
// [HarmonyPatch(typeof(AssetBundle))]
// class AssetBundlerPatch
// {
  // static IEnumerable<MethodBase> TargetMethods()
  // {

  //     // var asses = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("UnityEngine.AssetBundleModule") is true);
  //     Assembly assembly = Assembly.GetExecutingAssembly();
  //     Assembly assembly = asses.First();
  //     Plugin.Log.LogWarning($"patching:\n{assembly}");
  //     return AccessTools.GetTypesFromAssembly(assembly)
  //         .SelectMany(type => type.GetMethods())
  //         .Where(method => method.ReturnType != typeof(AssetBundle) && method.Name.StartsWith("Load"))
  //         // .Where(method => method.Name.StartsWith("Load"))
  //         .Cast<MethodBase>();

  // }

  // static void Prefix(object[] __args, MethodBase __originalMethod)
  // {
  //   // use dynamic code to handle all method calls
  //   var parameters = __originalMethod.GetParameters();
  //   Plugin.Log.LogInfo($"Method {__originalMethod.FullDescription()}:");
  //   for (var i = 0; i < __args.Length; i++)
  //     Plugin.Log.LogInfo($"{parameters[i].Name} of type {parameters[i].ParameterType} is {__args[i]}");
  // }

  // [HarmonyPrefix]
  // [HarmonyPatch(nameof(AssetBundle.LoadFromFile), new Type[] { typeof(string) })]
  // static void LoadFromFile_Postfix(string path, ref AssetBundle __result)
  // {
  //   try
  //   {

  //     Plugin.Log.LogInfo($"AssetBundle load!");

  //   }
  //   catch (System.Exception ex)
  //   {
  //     Plugin.Log.LogWarning($"Exception in patch of void Game::Init():\n{ex}");
  //   }
  // }



  // [HarmonyPrefix]
  // [HarmonyPatch(nameof(AssetBundle.Load), new Type[] { typeof(string) })]
  // static void Load_Postfix(string name, ref AssetBundle __result)
  // {
  //   try
  //   {
  //     Plugin.Log.LogInfo($"AssetBundle load!");
  //   }
  //   catch (System.Exception ex)
  //   {
  //     Plugin.Log.LogWarning($"Exception in patch of void Game::Init():\n{ex}");
  //   }
  // }

// }
// [HarmonyPatch(typeof(Application))]

// class AppPatch
// {


//   [HarmonyPostfix]
//   [HarmonyPatch(nameof(Application.LoadLevelAsync), new Type[] { typeof(string) })]
//   static void LoadLevelStringAsync_Postfix(string levelName, AsyncOperation __result)
//   {
//     try
//     {
//       Plugin.Log.LogInfo($"App Patch Level Loaded Async String {levelName}");
//     }
//     catch (Exception e)
//     {
//       Plugin.Log.LogInfo(e.ToString());
//     }
//   }

//   [HarmonyPostfix]
//   [HarmonyPatch(nameof(Application.LoadLevelAsync), new Type[] { typeof(int) })]
//   static void LoadLevelIntAsync_Postfix(int index, AsyncOperation __result)
//   {
//     try
//     {
//       Plugin.Log.LogInfo($"App Patch Level Loaded Async Index: {index}");
//     }
//     catch (Exception e)
//     {
//       Plugin.Log.LogInfo(e.ToString());
//     }
//   }

//   [HarmonyPostfix]
//   [HarmonyPatch(nameof(Application.LoadLevel), new Type[] { typeof(int) })]
//   static void LoadLevelInt_Postfix(int index, Application __result)
//   {
//     try
//     {
//       Plugin.Log.LogInfo($"App Patch Level Loaded Index: {index}");
//     }
//     catch (Exception e)
//     {
//       Plugin.Log.LogInfo(e.ToString());
//     }
//   }

//   [HarmonyPostfix]
//   [HarmonyPatch(nameof(Application.LoadLevel), new Type[] { typeof(string) })]
//   static void LoadLevelString_Postfix(string levelName, Application __result)
//   {
//     try
//     {
//       Plugin.Log.LogInfo($"App Patch Level Loaded String: {levelName}");
//     }
//     catch (Exception e)
//     {
//       Plugin.Log.LogInfo(e.ToString());
//     }
//   }
// }


// [HarmonyPatch(typeof(SceneManager))]
// public class Patch_SceneManger
// {
//   [HarmonyPostfix]
//   [HarmonyPatch(nameof(SceneManager.LoadScene), new Type[] { typeof(string) })]
//   static void LoadSceneString_PostFix(string sceneName)
//   {
//     Plugin.Log.LogInfo($"Patch_SceneManger SceneLoaded: {sceneName}");
//   }

//   [HarmonyPostfix]
//   [HarmonyPatch(nameof(SceneManager.LoadScene), new Type[] { typeof(int) })]
//   static void LoadSceneInt_PostFix(int sceneBuildIndex)
//   {
//     Plugin.Log.LogInfo($"Patch_SceneManger SceneLoaded: {sceneBuildIndex}");
//   }
// }