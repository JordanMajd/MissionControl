using UnityEngine;

using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Linq;
using System.Collections.Generic;
using SimpleJSON;

namespace MissionControl;


public class PartsManager : MonoBehaviour
{
  private Conf conf;
  private List<PartSO> partsList = new List<PartSO>();
  private List<GameObject> partsPrefabs = new List<GameObject>();
  private List<PartSO.Action> actionsList = new List<PartSO.Action>();
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

    if(MissionControlPlugin.MCConf.autoLoadAssetPacks.Value) {
      List<PartSO> newParts = LoadPartsFiles();
      AddParts(newParts);
    }    
  }

  public List<PartSO> LoadPartsFiles() {
    List<PartSO> loadedParts = new List<PartSO>();
    List<string> assetFileNames = Utils.Assets.GetJSONFiles();
    foreach(string fileName in assetFileNames) {
      loadedParts.AddRange(LoadPartsFile(fileName));
    }
    return loadedParts;
  }

  public List<PartSO> LoadPartsFile(string partsPath)
  {
    List<PartSO> loadedParts = new List<PartSO>();
    JSONNode partsPack = Utils.Assets.LoadJSON(partsPath);
    if(partsPack != null) {
      foreach (var part in partsPack["parts"])
      {
        loadedParts.Add(ParsePart(part));
      }
    }
    return loadedParts;
  }

  public PartSO ParsePart(JSONNode node)
  {
    // Get Parent Object
    string parentName = node["parent"];
    PartSO parent = conf.partList.parts.Where(part => part.name == parentName).First();

    string partType = node["type"];
    PartSO part = partType switch
    {
      "WheelSO" => CreateWheelSO(node, parent.Cast<WheelSO>()),
      "ServoSO" => CreateServoSO(node, parent.Cast<ServoSO>()),
      "PistonSO" => CreatePistonSO(node, parent.Cast<PistonSO>()),
      // "HornSO" => CreateHornSO(node, parent),
      _ => CreatePartSO<PartSO>(node, parent),
    };

    // save ref so it doesn't fall out of context
    partsList.Add(part);
    return part;
  }

  private T CreatePartSO<T>(JSONNode node, T parent) where T : PartSO
  {
    T newPart = ScriptableObject.CreateInstance<T>();
    // PartSO
    newPart.title = node["title"];
    newPart.description = node["description"];
    newPart.name = node["name"];
    newPart.titleLocTerm = node["titleLocTerm"];
    newPart.titleTranslation = node["titleTranslation"];
    newPart.cost = node["cost"].AsInt;
    newPart.mass = node["mass"].AsInt;
    newPart.includeInDemo = node["includeInDemo"].AsBool;
    newPart.order = node["order"].AsInt;
    newPart.ID = node["ID"].AsInt;

    // Prefab
    // instantiate and store a ref to prevent from going out of context
    GameObject prefab = GameObject.Instantiate(parent.prefab);
    partsPrefabs.Add(prefab);
    prefab.hideFlags = HideFlags.HideAndDontSave;
    prefab.name = node["name"];

    if (node["scale"] != null)
    {
      float scale = node["scale"].AsFloat;
      prefab.transform.localScale = new Vector3(scale, scale, scale);
    }
    if (node["mesh"] != null && node["meshBundle"] != null)
    {
      MeshFilter mf = prefab.GetComponentInChildren<MeshFilter>();
      if (mf != null)
      {
        mf.mesh = Utils.Assets.LoadAsset<Mesh>(node["meshBundle"], node["mesh"]);
      }
      // MeshRenderer mr = prefab.GetComponentInChildren<MeshRenderer>();
      // if(mr != null) {
      //   Material testMaterial = Utils.Assets.LoadAsset<Material>(node["meshBundle"], "TestMat");
      //   mr.SetMaterial(testMaterial);
      // }
    }
    newPart.prefab = prefab;

    // inherits
    newPart.attachmentPoints = parent.attachmentPoints;
    newPart.meshGOPaths = parent.meshGOPaths;
    newPart.iconTex = parent.iconTex;
    newPart.actions = parent.actions;

    return newPart;
  }

  // Doesn't work :C
  private HornSO CreateHornSO(JSONNode node, HornSO parent)
  {
    HornSO newHorn = CreatePartSO<HornSO>(node, parent);
    List<PartSO.Action> lilActions = new List<PartSO.Action>();

    PartSO.Action action = new PartSO.Action();
    action.action = Control.Action.Horn;
    action.actionName = "Honk";
    action.locTerm = "Actions/Horn";
    lilActions.Add(action);
    newHorn.actions = lilActions.ToArray();
    newHorn.SetActions();

    // don't let it fall out of reference
    actionsList.Add(action);

    return newHorn;
  }


  private PistonSO CreatePistonSO(JSONNode node, PistonSO parent)
  {
    PistonSO newPiston = CreatePartSO<PistonSO>(node, parent);
    newPiston.pistonSpring = node["pistonSpring"].AsFloat;
    newPiston.pistonDamper = node["pistonDamper"].AsFloat;
    newPiston.pistonMaxExtent = node["pistonMaxExtent"].AsFloat;
    return newPiston;
  }

  private ServoSO CreateServoSO(JSONNode node, ServoSO parent)
  {
    ServoSO newServo = CreatePartSO<ServoSO>(node, parent);
    newServo.servoSpring = node["servoSpring"].AsFloat;
    newServo.servoDamper = node["servoDamper"].AsFloat;
    return newServo;
  }

  private WheelSO CreateWheelSO(JSONNode node, WheelSO parent)
  {
    WheelSO newWheel = CreatePartSO<WheelSO>(node, parent);

    // WheelSO
    newWheel.motorForce = node["motorForce"].AsFloat;
    newWheel.throttleSpeed = node["throttleSpeed"].AsFloat;
    newWheel.sideFrictionCoef = node["sideFrictionCoef"].AsFloat;
    newWheel.slipForceBoost = node["slipForceBoost"].AsFloat;
    newWheel.stickyForce = node["stickyForce"].AsFloat;
    newWheel.wheelTreadFreq = node["wheelTreadFreq"].AsFloat;
    newWheel.wheelTreadOffset = node["wheelTreadOffset"].AsFloat;
    newWheel.wheelDamper = node["wheelDamper"].AsFloat;
    newWheel.wheelSpring = node["wheelSpring"].AsFloat;
    newWheel.applyForceRadius = node["applyForceRadius"].AsFloat;
    newWheel.wheelWidth = node["wheelWidth"].AsFloat;
    newWheel.wheelRadius = node["wheelRadius"].AsFloat;

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