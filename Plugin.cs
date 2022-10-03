// Running under Unity 2020.3.32f1
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

using HarmonyLib;

using UniverseLib.Runtime.Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

using System;

using System.IO;

namespace HelloMars;


// XUnity.ResourceRedirector to redirect to other resources

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{

  internal static new ManualLogSource Log;
  internal static Game Game;

  internal static Harmony Harmony;
  public override void Load()
  {
    // set up logger
    Plugin.Log = base.Log;
    Plugin.Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

    // test code
    Assembly assembly = Assembly.GetExecutingAssembly();
    // var names = assembly.GetManifestResourceNames();
    // foreach(var name in names) {
    //   Plugin.Log.LogInfo($"Name: {name}");
    // }
    // var stream = assembly.GetManifestResourceStream("HelloMars.Resources.test.txt");
    // StreamReader sr = new StreamReader(stream);
    // var str = sr.ReadToEnd();
    // Plugin.Log.LogInfo(str);


    // run patches
    Harmony = new Harmony("com.hellomars.patch");
    Harmony.PatchAll();
    // custom patches
    // Patch_Resources.Init();
    Plugin.Log.LogInfo($"Patches applied");

    HelloComponent sc = AddComponent<HelloComponent>();
    // if we wanted to late mount component
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
  GameObject minifig;

  private UnityAction<Scene, LoadSceneMode> OnSceneLoaded = (UnityAction<Scene, LoadSceneMode>)((scene, mode) =>
  {
    Plugin.Log.LogInfo($"Scene {scene.name} is loaded!");
  });
  public void Awake()
  {
    Plugin.Log.LogInfo("Awake");
    gameObject.name = "Hello Mars";
    gameObject.hideFlags = HideFlags.HideAndDontSave;

    if (OnSceneLoaded != null)
    {
      SceneManager.add_sceneLoaded(OnSceneLoaded);
    }
    // Plugin.Log.LogInfo($"Path {Application.dataPath}");
    // AssetBundle.LoadFromFileAsync("tire");
    // AssetBundle.LoadFromFileAsync(Path.Combine(Application.dataPath, "tire"));
    // AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, "tire"));
    // string path = Path.Combine(BepInEx.Paths.PluginPath, "HelloMars/Assets/tire");

    // if (!File.Exists(path))
    // {
    //   Plugin.Log.LogInfo($"Path doesn't exists {path}");
    // } else {
    //   Plugin.Log.LogInfo($"Path exists! {path}");
    // }
    // AssetBundle.LoadFromFileAsync(path);


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
        bigWheel.titleLocTerm = "Parts/BigWheel";
        bigWheel.titleTranslation = "Big Wheel";
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
        // AssetBundle
// [Error  :     Unity] Unable to read header from archive file: C:/Program Files (x86)/Steam/steamapps/common/Mars First Logistics Demo/Mars First Logistics_Data/tire.unity3d
// [Error  :UnityExplorer] [Unity] Unable to read header from archive file: C:/Program Files (x86)/Steam/steamapps/common/Mars First Logistics Demo/Mars First Logistics_Data/tire.unity3d
// [Error  :     Unity] Failed to read data for the AssetBundle 'C:\Program Files (x86)\Steam\steamapps\common\Mars First Logistics Demo\Mars First Logistics_Data\tire.unity3d'.
// [Error  :UnityExplorer] [Unity] Failed to read data for the AssetBundle 'C:\Program Files (x86)\Steam\steamapps\common\Mars First Logistics Demo\Mars First Logistics_Data\tire.unity3d'.

        // AssetBundle bundle = AssetBundle.LoadFromFile_Internal(Path.Combine(Application.streamingAssetsPath, "tire.tire"), 0, 0);
        // if(bundle == null) {
        //   Plugin.Log.LogError("YEEEEEEE");
        //   return;
        // }
        // UnityEngine.Object tire = bundle.LoadAsset("Tire_v1_L");
        string path = Path.Combine(BepInEx.Paths.GameDataPath, "legobundle");
        Plugin.Log.LogInfo($"Opening path {path}");
        UniverseLib.AssetBundle ab = UniverseLib.AssetBundle.LoadFromFile(path);
        var mfGO = ab.LoadAsset<GameObject>("Minifig");
        // var mf = GameObject.Find("Minifig");
        minifig = Instantiate(mfGO);


        // Plugin.Log.LogInfo("0");
        // Assembly assembly = Assembly.GetExecutingAssembly();
        // // // UniverseLib.AssetBundle assBundle;
        // // Plugin.Log.LogInfo("1");
        // using (var stream = assembly.GetManifestResourceStream("HelloMars.Resources.test.txt"))
        // {
        //   byte[] buff = new byte[stream.Length];
        //   stream.Read(buff, 0, buff.Length);
        //   UniverseLib.AssetBundle assBun = UniverseLib.AssetBundle.LoadFromMemory(buff, 0);
        //   if (stream != null)
        //   {
        //     Plugin.Log.LogInfo("1.5");
        //   }
        //   Plugin.Log.LogInfo("2");
        //   var il2Stream = new Il2CppSystem.IO.MemoryStream();
        //   // byte[] buff = new byte[stream.Length];
        //   // stream.Read(buff, 0, 0);
        //   // var il2Buff = new Il2CppStructArray<byte>(buff.Length);
        //   // for (var i = 0; i < buff.Length; i++)
        //   // {
        //   //   il2Buff[i] = buff[i];
        //   // }
        //   // IntPtr* ptr = IL2CPP.Il2CppObjectBaseToPtr(stream);
        //   // IntPtr ptr - stream
        //   // Plugin.Log.LogInfo($"canWrite { il2Stream.Length}");
        //   // il2Stream.Write(il2Buff, 0, 0);
        //   int byt;
        //   while ((byt = stream.ReadByte()) != -1)
        //   { 
        //     il2Stream.WriteByte((byte) byt);
        //     // il2Stream.BlockingBeginWrite
        //   }
        //   // AssetBundle.LoadFromStreamAsync(il2Stream);
        //   Plugin.Log.LogInfo("3");
        //   AssetBundle.LoadFromStreamAsync(il2Stream, 0, 0);
        //   // IntPtr ptr = ICallManager.GetICallUnreliable<d_LoadFromStream>(
        //   //         "UnityEngine.AssetBundle::LoadFromStream_Internal",
        //   //         "UnityEngine.AssetBundle::LoadFromStream")
        //   //     .Invoke(il2Stream, 0u, 0UL);
        //   Plugin.Log.LogInfo("4");
        //   // assBundle = ptr != IntPtr.Zero ? new UniverseLib.AssetBundle(ptr) : null;
        // }
        Plugin.Log.LogInfo("5");
        
        // Prefab
        bigWheelPrefab = GameObject.Instantiate(oldPart.prefab);
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
        bigWheel.AddToGlobalDict();


        // localization, there's gotta be a better way
        // var rm = GetComponent("I2ResourceManager/ResourceManager");
        bigWheel.GetTranslations();

        if (Plugin.Game != null)
        {
          Plugin.Game.partsInventory.UnlockAllParts();
          // Plugin.Game.contracts
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

[HarmonyPatch(typeof(PartSO), nameof(PartSO.GetTranslations))]
class Patch_PartSOTranslation
{
  static void Postix(ref PartSO __instance)
  {
    Plugin.Log.LogInfo($"GetTrans");
    if (__instance.titleTranslation.StartsWith("!"))
    {
      Plugin.Log.LogInfo($"Inside");
      __instance.titleTranslation = __instance.title;
    }
  }
}


// public class Patch_Resources
// {
//   [HarmonyPostfix]
//   static void Postfix1(ref string path)
//   {
//     Plugin.Log.LogWarning($"Resources.Load path: {path}");
//   }

//   [HarmonyPrefix]
//   static bool Prefix2(ref UnityEngine.Object __result, ref string path, ref Il2CppSystem.Type systemTypeInstance)
//   {
//     Plugin.Log.LogWarning($"Resources.Load path: {path}");
//     if (path.Contains("MYTHING"))
//     {
//       // __result = Items.Headhunter.Sprite;
//       // return false;
//     }
//     return true;
//   }

//   [HarmonyPostfix]
//   static void Postfix2(ref string path, ref Il2CppSystem.Type systemTypeInstance)
//   {
//     Plugin.Log.LogInfo($"Resources.Load Path: {path} | Type : {systemTypeInstance.ToString()}");
//   }

//   public static void Init()
//   {
//     Plugin.Log.LogInfo("=============================");
//     Plugin.Log.LogInfo("INIT");
//     var bf = (BindingFlags)(-1);

//     var resourcesLoadMethod = typeof(Resources).GetMethods(bf).Where(m => m.Name == nameof(Resources.Load) && m.GetParameters().Length == 1).ToArray()[0];
//     var postfix = new HarmonyMethod(typeof(Patch_Resources).GetMethod(nameof(Postfix1), bf));
//     HelloMars.Plugin.Harmony.Patch(resourcesLoadMethod, null, postfix);

//     var resourcesLoadMethod2 = typeof(Resources).GetMethods(bf).Where(m => m.Name == nameof(Resources.Load) && m.GetParameters().Length == 2).ToArray()[0];
//     var prefix2 = new HarmonyMethod(typeof(Patch_Resources).GetMethod(nameof(Prefix2), bf));
//     var postfix2 = new HarmonyMethod(typeof(Patch_Resources).GetMethod(nameof(Postfix2), bf));
//     HelloMars.Plugin.Harmony.Patch(resourcesLoadMethod2, prefix2, postfix2);
//   }
// }

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