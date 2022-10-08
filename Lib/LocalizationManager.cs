using UnityEngine;

using I2.Loc;
using System.Linq;
namespace MissionControl;
using System;
public class LocalizationManager : MonoBehaviour
{

  public LanguageSourceData LSData;
  public void Awake() {
    gameObject.name = "LocalizationManager";
    gameObject.hideFlags = HideFlags.HideAndDontSave;
  }
  public void Start()
  {
    GameObject resManGO = GameObject.Find("I2ResourceManager");
    if(resManGO != null){
      ResourceManager res = resManGO.GetComponent<ResourceManager>();
      LanguageSourceAsset sourceAsset = res.mResourcesCache["I2Languages"].Cast<LanguageSourceAsset>();
      LSData = sourceAsset.SourceData;
      ImportLocalization("Parts");
    }
  }

  // TODO real line endings
  public void ImportLocalization(string category) {
    string newlocCSV = Utils.Assets.LoadCSV();

    MissionControlPlugin.Log.LogInfo(newlocCSV);
    // remove header from new locCSV
    // var split = newlocCSV.Split(Environment.NewLine).Skip(1).ToArray();
    // var csvFirstLIneRemoved = string.Join(Environment.NewLine, split);
    // string oldLocCSV = LSData.Export_CSV(category);
    LSData.Import_CSV(category, newlocCSV, eSpreadsheetUpdateMode.AddNewTerms);

  }
}