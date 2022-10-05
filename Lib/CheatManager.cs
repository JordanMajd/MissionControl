
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

using UniverseLib.UI.Panels;
using UniverseLib.UI.Models;
using UniverseLib.Input;
namespace MissionControl;


public class CheatManager : MonoBehaviour
{

  public void Start()
  {
    CheatUIManager.Init(this);
  }

  public void Update()
  {
    if (InputManager.GetKeyDown(KeyCode.F8))
      CheatUIManager.ShowMenu();
  }


  public void DisableGravity()
  {
    GameObject vehicle = GameObject.Find("vehicle");
    ArticulationBody[] bodies = vehicle.GetComponentsInChildren<ArticulationBody>();
    foreach (var body in bodies)
    {
      body.useGravity = false;
    }
  }


  public void UnlockAllParts()
  {
    Conf.g.partsInventory.UnlockAllParts();
  }

  public void SetPartsTotal(int count)
  {
    Conf.g.partsInventory.UnlockAllParts();
    foreach (var entry in Conf.g.partsInventory.unlockedParts)
    {
      entry.Value.total = count;
    }
  }

}

public class CheatUIManager : PanelBase
{

  public static void ShowMenu()
  {
    UniversalUI.SetUIActive(MyPluginInfo.PLUGIN_GUID, true);
    Instance.SetActive(true);
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
  public override int MinHeight => 200;

  public override Vector2 DefaultAnchorMin => new(0.2f, 0.02f);
  public override Vector2 DefaultAnchorMax => new(0.8f, 0.98f);

  public static void Init(CheatManager _cheatManager)
  {
    uiBase = UniversalUI.RegisterUI(MyPluginInfo.PLUGIN_GUID, null);
    cheatManager = _cheatManager;
    new CheatUIManager(uiBase);
  }

  protected override void ConstructPanelContent()
  {
    UIFactory.SetLayoutGroup<VerticalLayoutGroup>(ContentRoot, true, false, true, true);
    ButtonRef gravButton = UIFactory.CreateButton(ContentRoot, "DisableGravityButton", "Disable Gravity");
    UIFactory.SetLayoutElement(gravButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
    gravButton.OnClick += OnGravityButtonClick;

    ButtonRef unlockButton = UIFactory.CreateButton(ContentRoot, "UnlockPartsButton", "Unlock All Parts");
    UIFactory.SetLayoutElement(unlockButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
    gravButton.OnClick += OnUnlockAllPartsButton;


    ButtonRef givePartsButton = UIFactory.CreateButton(ContentRoot, "GivePartsButton", "Give 9999 of Each Part");
    UIFactory.SetLayoutElement(givePartsButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
    gravButton.OnClick += OnGivePartsButtonClick;
  }

  protected static void OnGravityButtonClick()
  {
    if (cheatManager != null)
    {
      MissionControlPlugin.Log.LogInfo("OnGravityButtonClick");
      cheatManager.DisableGravity();
    }
  }

  protected static void OnGivePartsButtonClick()
  {
    if (cheatManager != null)
    {
      MissionControlPlugin.Log.LogInfo("OnGivePartsButtonClick");
      cheatManager.SetPartsTotal(9999);
    }
  }

  protected static void OnUnlockAllPartsButton()
  {
    if (cheatManager != null)
    {
      MissionControlPlugin.Log.LogInfo("OnUnlockAllPartsButton");
      cheatManager.UnlockAllParts();
    }
  }
}