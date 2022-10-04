using UnityEngine;

using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Linq;
using System.Collections.Generic;
using System;
using SimpleJSON;

namespace MissionControl;


public class PartsManager : MonoBehaviour
{
  private Conf conf;
  private List<PartSO> partsList = new List<PartSO>();
  private List<GameObject> partsPrefabs = new List<GameObject>();

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
    string partType = node["type"];

    // Get Parent Object
    string parentName = node["parent"];
    PartSO parent = conf.partList.parts.Where(part => part.name == parentName).First();

    // Setup Prefab
    // instantiate and store a ref to prevent from going out of context
    GameObject prefab = GameObject.Instantiate(parent.prefab);
    partsPrefabs.Add(prefab);
    prefab.hideFlags = HideFlags.HideAndDontSave;
    prefab.name = node["name"];

    if(node["scale"] != null) {
      float scale = node["scale"].AsFloat;
      prefab.transform.localScale = new Vector3(scale, scale, scale);
    }
    if(node["mesh"] != null && node["meshBundle"] != null) {
      MeshFilter mf = prefab.GetComponentInChildren<MeshFilter>();
      if (mf != null)
      {
        mf.mesh = Utils.Assets.LoadAsset<Mesh>(node["meshBundle"], "tire_v3");
      }
    }


    PartSO part = partType switch
    {
      "WheelSO" => CreateWheelSO(node, parent.Cast<WheelSO>(), prefab),
      _ => CreatePartSO<PartSO>(node, parent, prefab),
    };

    // save ref so it doesn't fall out of context
    partsList.Add(part);
    return part;
  }

  private T CreatePartSO<T>(JSONNode wheelNode, PartSO parent, GameObject prefab) where T : PartSO
  {
    T newPart = ScriptableObject.CreateInstance<T>();
    // PartSO
    newPart.title = wheelNode["title"];
    newPart.description = wheelNode["description"];
    newPart.name = wheelNode["name"];
    newPart.titleLocTerm = wheelNode["titleLocTerm"];
    newPart.titleTranslation = wheelNode["titleTranslation"];
    newPart.cost = wheelNode["cost"].AsInt;
    newPart.mass = wheelNode["mass"].AsInt;
    newPart.includeInDemo = wheelNode["includeInDemo"].AsBool;
    newPart.order = wheelNode["order"].AsInt;
    newPart.ID = wheelNode["ID"].AsInt;

    // Prefab
    newPart.prefab = prefab;

    // inherits
    newPart.attachmentPoints = parent.attachmentPoints;
    newPart.meshGOPaths = parent.meshGOPaths;
    newPart.iconTex = parent.iconTex;
    newPart.actions = parent.actions;

    return newPart;
  }

  private WheelSO CreateWheelSO(JSONNode wheelNode, WheelSO parent, GameObject prefab)
  {
    WheelSO newWheel = CreatePartSO<WheelSO>(wheelNode, parent, prefab);

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