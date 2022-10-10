using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Panels;
using UniverseLib.UI.Models;
using UniverseLib.Input;
namespace MissionControl;

// To Add Cheats
// - SetSaveFile
public class CheatManager : MonoBehaviour
{
  private bool currentGravity = true;

  public void Awake()
  {
    gameObject.name = "CheatManager";
    gameObject.hideFlags = HideFlags.HideAndDontSave;
  }
  public void Start()
  {
    CheatUIManager.Init(this);
  }

  public void Update()
  {
    if (InputManager.GetKeyDown(KeyCode.F8))
      CheatUIManager.ToggleMenu();
  }

  public void ToggleGravity()
  {
    // could access Conf.g.player.vehicle.
    GameObject vehicle = GameObject.Find("vehicle");
    ArticulationBody[] bodies = vehicle.GetComponentsInChildren<ArticulationBody>();
    foreach (var body in bodies)
    {
      body.useGravity = !currentGravity;
    }
    currentGravity = !currentGravity;
  }

  public void UnlockAllParts()
  {
    Conf.g.partsInventory.UnlockAllParts();
  }

  public void GiveParts(int count)
  {
    foreach (var entry in Conf.g.partsInventory.unlockedParts)
    {
      // entry.Value.total = count;
      Conf.g.partsInventory.AddPart(entry.Value.id, count, false);
    }

  }
  public void GiveFunds(int amount)
  {
    Conf.g.player.money += amount;
  }

  public void SetMaxSpeed(float maxSpeed)
  {
    GameObject confGO = GameObject.Find("Conf");
    Conf conf = confGO.GetComponent<Conf>();
    conf.maxGroundSpeed = maxSpeed;
    conf.maxAirSpeed = maxSpeed;
    Conf.g.player.vehicle.maxSpeed = maxSpeed;
    // Conf.g.player.vehicle.CapSpeed();
  }


  public void SetGridScale(float scale)
  {
    // VehicleEditor.maxBuildExtent = scale;
    Conf.g.editor.grid.transform.localScale = new Vector3(scale, scale, scale);
    if (scale != 0)
    {
      Conf.g.editor.editorCam.minScale = 1 / scale;
    }
  }
}

public class CheatUIManager : PanelBase
{
  public static void ToggleMenu()
  {
    if(Instance.Enabled)
      HideMenu();
    else
      ShowMenu();
  }

  public static void ShowMenu()
  {
    UniversalUI.SetUIActive(MyPluginInfo.PLUGIN_GUID, true);
    Instance.SetActive(true);
  }

  public static void HideMenu()
  {
    UniversalUI.SetUIActive(MyPluginInfo.PLUGIN_GUID, false);
    Instance.SetActive(false);
  }

  static CheatManager cheatManager;
  static UIBase uiBase;
  public static CheatUIManager Instance { get; internal set; }
  public CheatUIManager(UIBase owner) : base(owner)
  {
    Instance = this;
  }
  public override string Name => "Mission Control: Cheat Manager";
  public override int MinWidth => 200;
  public override int MinHeight => 300;

  public override Vector2 DefaultAnchorMin => new(0.2f, 0.02f);
  public override Vector2 DefaultAnchorMax => new(0.4f, 0.04f);

  public static void Init(CheatManager _cheatManager)
  {
    uiBase = UniversalUI.RegisterUI(MyPluginInfo.PLUGIN_GUID, null);
    cheatManager = _cheatManager;
    new CheatUIManager(uiBase);
  }

  protected override void ConstructPanelContent()
  {
    UIFactory.SetLayoutGroup<VerticalLayoutGroup>(ContentRoot, true, false, true, true);

    ButtonRef toggleGravButton = UIFactory.CreateButton(ContentRoot, "ToggleGravityButton", "Toggle Gravity");
    UIFactory.SetLayoutElement(toggleGravButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
    toggleGravButton.OnClick += OnToggleGravityButtonClick;

    ButtonRef unlockButton = UIFactory.CreateButton(ContentRoot, "UnlockPartsButton", "Unlock All Parts");
    UIFactory.SetLayoutElement(unlockButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
    unlockButton.OnClick += OnUnlockAllPartsButtonClick;

    ButtonRef givePartsButton = UIFactory.CreateButton(ContentRoot, "GivePartsButton", "Give 9999 of Each Part");
    UIFactory.SetLayoutElement(givePartsButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
    givePartsButton.OnClick += OnGivePartsButtonClick;

    ButtonRef giveFundsButton = UIFactory.CreateButton(ContentRoot, "GiveFundsButton", "Give $999,999");
    UIFactory.SetLayoutElement(giveFundsButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
    giveFundsButton.OnClick += OnGiveFundsButtonClick;

    ButtonRef maxSpeedButton = UIFactory.CreateButton(ContentRoot, "MaxSpeedButton", "Set Max Speed 9999.9");
    UIFactory.SetLayoutElement(maxSpeedButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
    maxSpeedButton.OnClick += OnMaxSpeedButtonClick;

    ButtonRef removeBuildLimitButton = UIFactory.CreateButton(ContentRoot, "RemoveBuildLimitButton", "Remove Build Limit");
    UIFactory.SetLayoutElement(removeBuildLimitButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
    removeBuildLimitButton.OnClick += OnRemoveBuildLimitClick;
  }

  protected static void OnRemoveBuildLimitClick()
  {
    if (cheatManager != null)
    {
      cheatManager.SetGridScale(999);
    }
  }

  protected static void OnToggleGravityButtonClick()
  {
    if (cheatManager != null)
    {
      cheatManager.ToggleGravity();
    }
  }

  protected static void OnGivePartsButtonClick()
  {
    if (cheatManager != null)
    {
      cheatManager.GiveParts(9999);
    }
  }

  protected static void OnGiveFundsButtonClick()
  {
    if (cheatManager != null)
    {
      cheatManager.GiveFunds(999999);
    }
  }

  protected static void OnUnlockAllPartsButtonClick()
  {
    if (cheatManager != null)
    {
      cheatManager.UnlockAllParts();
    }
  }
  protected static void OnMaxSpeedButtonClick()
  {
    if (cheatManager != null)
    {
      cheatManager.SetMaxSpeed(99999f);
    }
  }
}