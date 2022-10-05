
using UnityEngine;
namespace MissionControl;


public class CheatManager : MonoBehaviour
{

  public void Start()
  {
    UnlockAllParts();
    SetPartsTotal(9999);
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

  public void SetPartsTotal(int count){
    Conf.g.partsInventory.UnlockAllParts();
    foreach (var entry in Conf.g.partsInventory.unlockedParts) {
      entry.Value.total = count;
    }
  }

}