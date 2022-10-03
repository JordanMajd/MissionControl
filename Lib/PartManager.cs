// Running under Unity 2020.3.32f1
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

namespace MissionControl;

// [Unity] Not allowed to access vertices on mesh 'Model' (isReadable is false; Read/Write must be enabled in import settings)
// Cannot combine mesh that does not allow access: Model
// [Unity] Cannot combine mesh that does not allow access: Model
// [Warning:     Unity] Cannot combine mesh that does not allow access: Model
// [Error  :     Unity] Material 'Part (Instance)' with Shader 'Custom/Part' doesn't have a texture property '_MainTex'
// [Unity] Material 'Part (Instance)' with Shader 'Custom/Part' doesn't have a texture property '_MainTex'
public class PartsManager : MonoBehaviour
{
  WheelSO bigWheel;
  GameObject bigWheelPrefab;

  List<GameObject> spawnedObjects = new List<GameObject>();

  private UnityAction<Scene, LoadSceneMode> OnSceneLoaded = (UnityAction<Scene, LoadSceneMode>)((scene, mode) =>
  {
    MissionControlPlugin.Log.LogInfo($"Scene {scene.name} is loaded!");
  });
  public void Awake()
  {
    MissionControlPlugin.Log.LogInfo("Awake");
    gameObject.name = "PartsManager";
    gameObject.hideFlags = HideFlags.HideAndDontSave;
    if (OnSceneLoaded != null)
    {
      SceneManager.add_sceneLoaded(OnSceneLoaded);
    }
  }
  void Start()
  {
    MissionControlPlugin.Log.LogInfo("Start");
    GameObject gameConf = GameObject.Find("Conf");
    if (gameConf != null)
    {
      Conf confComp = gameConf.GetComponent<Conf>();

      if (confComp != null)
      {
        MissionControlPlugin.Log.LogInfo("Creating BigWheel");


        // Utils.Assets.LoadJSON("example-pack.json", );

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

        // inherit 
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
        bigWheelPrefab = GameObject.Instantiate(oldPart.prefab);
        bigWheelPrefab.hideFlags = HideFlags.HideAndDontSave;
        bigWheelPrefab.name = "BigWheel";
        bigWheelPrefab.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
        MeshFilter mf = bigWheelPrefab.GetComponentInChildren<MeshFilter>();
        if(mf != null) {
          mf.mesh = Utils.Assets.LoadAsset<Mesh>("examplebundle", "tire_v3");
        }
        MeshRenderer mr = bigWheelPrefab.GetComponentInChildren<MeshRenderer>();

        if(mr != null) {
          var oldMr = oldPart.prefab.GetComponentInChildren<MeshRenderer>();
          var tex = oldMr.material.GetTexture("_MainTex");
          mr.material.SetTexture("_MainText", tex);
        }
        
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

        // TBD localization, there's gotta be a better way
        // var rm = GetComponent("I2ResourceManager/ResourceManager");
        bigWheel.GetTranslations();

        // TODO move to cheats manager
        if (Conf.g != null)
        {
          Conf.g.partsInventory.UnlockAllParts();
          MissionControlPlugin.Log.LogInfo("All parts unlocked");
        }
      }
    }
  }
}