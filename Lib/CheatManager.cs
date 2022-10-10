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
  
  struct CheatButton
  {
    public string name;
    public string text;
    public int height;
    public System.Action action;

    public CheatButton(string buttonName, string buttonText, int buttonHeight, System.Action buttonAction)
    {
      name = buttonName;
      text = buttonText;
      height = buttonHeight;
      action = buttonAction;
    }
  }

  static CheatButton[] cheatButtons = new CheatButton[]{
    new CheatButton("ToggleGravityButton", "Toggle Gravity", 35, OnToggleGravityButtonClick),
    new CheatButton("UnlockPartsButton", "Unlock All Parts", 35, OnUnlockAllPartsButtonClick),
    new CheatButton("GivePartsButton", "Give 9999 of Each Part", 35, OnGivePartsButtonClick),
    new CheatButton("GiveFundsButton", "Give $999,999", 35, OnGiveFundsButtonClick),
    new CheatButton("MaxSpeedButton", "Set Max Speed 9999.9", 35, OnMaxSpeedButtonClick),
    new CheatButton("RemoveBuildLimitButton", "Remove Build Limit", 35, OnRemoveBuildLimitButtonClick),
  };

  static System.Func<int> GetTotalButtonHeight = () =>
  {
    int sum = 33;
    foreach(CheatButton button in cheatButtons){
      sum += button.height + 2;
    }
    return sum;
  };

  static CheatManager cheatManager;
  static UIBase uiBase;
  public static CheatUIManager Instance { get; internal set; }
  public CheatUIManager(UIBase owner) : base(owner)
  {
    Instance = this;
  }
  public override string Name => "Mission Control: Cheat Manager";
  public override int MinWidth => 200;
  public override int MinHeight => GetTotalButtonHeight();

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

    foreach(CheatButton button in cheatButtons){
      ButtonRef buttonRef = UIFactory.CreateButton(ContentRoot, button.name, button.text);
      UIFactory.SetLayoutElement(buttonRef.Component.gameObject, minHeight: button.height, flexibleHeight: 0, flexibleWidth: 9999);
      buttonRef.OnClick += button.action;
    }
  }

  protected static void OnRemoveBuildLimitButtonClick()
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