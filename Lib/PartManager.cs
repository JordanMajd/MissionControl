using UnityEngine;

using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Linq;
using System.Collections.Generic;
using SimpleJSON;

namespace MissionControl;
public class PartsManager : MonoBehaviour
{
  private Conf conf;
  List<PartSO> partsList = new List<PartSO>();
  List<GameObject> partsPrefabs = new List<GameObject>();

  public void Awake()
  {
    gameObject.name = "PartsManager";
    gameObject.hideFlags = HideFlags.HideAndDontSave;
  }
  void Start()
  {
    GameObject gameConf = GameObject.Find("Conf");
    if (gameConf != null)
    {
      conf = gameConf.GetComponent<Conf>();
    }
    // TODO only do if config is set to load example asset pack
    var newParts = this.LoadPartsFile("example-pack.json");
    AddParts(newParts);

    // TODO move to cheats manager
    if (Conf.g != null)
    {
      Conf.g.partsInventory.UnlockAllParts();
      MissionControlPlugin.Log.LogInfo("All parts unlocked");
    }
  }

  public List<PartSO> LoadPartsFile(string partsPath)
  {
    List<PartSO> loadedParts = new List<PartSO>();
    JSONNode partsPack = Utils.Assets.LoadJSON(partsPath);
    foreach (var part in partsPack["parts"])
    {
      loadedParts.Add(ParsePart(part));
    }
    return loadedParts;
  }

  public PartSO ParsePart(JSONNode node)
  {
    // Get Parent Object
    string parentName = node["parent"];
    PartSO c = conf.partList.parts.Where(part => part.name == parentName).First();
    WheelSO parent = c.Cast<WheelSO>();

    // Setup Prefab
    GameObject prefab = GameObject.Instantiate(parent.prefab);
    // keep reference to prevent it from going out of context
    partsPrefabs.Add(prefab);
    prefab.hideFlags = HideFlags.HideAndDontSave;
    prefab.name = node["name"];
    prefab.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
    MeshFilter mf = prefab.GetComponentInChildren<MeshFilter>();
    if (mf != null)
    {
      mf.mesh = Utils.Assets.LoadAsset<Mesh>("examplebundle", "tire_v3");
    }
    MeshRenderer mr = prefab.GetComponentInChildren<MeshRenderer>();
    if (mr != null)
    {
      var parentMR = parent.prefab.GetComponentInChildren<MeshRenderer>();
      var tex = parentMR.material.GetTexture("_MainTex");
      mr.material.SetTexture("_MainText", tex);
    }

    // Get new
    WheelSO wheel = CreateWheelSOInstance(node, parent, prefab);
    // Stash ref so it doesn't fall out of context
    partsList.Add(wheel);
    return wheel;
  }


  private WheelSO CreateWheelSOInstance(JSONNode wheelNode, WheelSO parent, GameObject prefab)
  {
    WheelSO newWheel = ScriptableObject.CreateInstance<WheelSO>();

    // PartSO
    newWheel.title = wheelNode["title"];
    newWheel.description = wheelNode["description"];
    newWheel.name = wheelNode["name"];
    newWheel.titleLocTerm = wheelNode["titleLocTerm"];
    newWheel.titleTranslation = wheelNode["titleTranslation"];
    newWheel.cost = wheelNode["cost"].AsInt;
    newWheel.mass = wheelNode["mass"].AsInt;
    newWheel.includeInDemo = wheelNode["includeInDemo"].AsBool;
    newWheel.order = wheelNode["order"].AsInt;
    newWheel.ID = wheelNode["ID"].AsInt;

    // Prefab
    newWheel.prefab = prefab;

    // PartSO inherits 
    newWheel.attachmentPoints = parent.attachmentPoints;
    newWheel.meshGOPaths = parent.meshGOPaths;
    newWheel.iconTex = parent.iconTex;
    newWheel.actions = parent.actions;

    // WheelSO
    newWheel.motorForce = wheelNode["motorForce"].AsFloat;
    newWheel.throttleSpeed = wheelNode["throttleSpeed"].AsFloat;
    newWheel.sideFrictionCoef = wheelNode["sideFrictionCoef"].AsFloat;
    newWheel.slipForceBoost = wheelNode["slipForceBoost"].AsFloat;
    newWheel.stickyForce = wheelNode["stickyForce"].AsFloat;
    newWheel.wheelTreadFreq = wheelNode["wheelTreadFreq"].AsFloat;
    newWheel.wheelTreadOffset = wheelNode["wheelTreadOffset"].AsFloat;
    newWheel.wheelDamper = wheelNode["wheelDamper"].AsFloat;
    newWheel.wheelSpring = wheelNode["wheelSpring"].AsFloat;
    newWheel.applyForceRadius = wheelNode["applyForceRadius"].AsFloat;
    newWheel.wheelWidth = wheelNode["wheelWidth"].AsFloat;
    newWheel.wheelRadius = wheelNode["wheelRadius"].AsFloat;

    // WheelSO Inherits
    newWheel.impactEventPath = parent.impactEventPath;
    newWheel.sideFrictionCurve = parent.sideFrictionCurve;

    return newWheel;
  }

  public void AddParts(List<PartSO> newParts)
  {
    // register new parts with game config
    foreach (var part in newParts)
    {
      part.GetTranslations();
      part.AddToGlobalDict();
    }

    // add to conf parts list
    Il2CppReferenceArray<PartSO> existingParts = conf.partList.parts;
    foreach (var part in existingParts)
    {
      newParts.Add(part);
    }
    conf.partList.parts = new(newParts.ToArray());
  }
}