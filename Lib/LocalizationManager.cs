using UnityEngine;

using I2.Loc;
using Il2CppSystem.Collections.Generic;

namespace MissionControl;
public class LocalizationManager : MonoBehaviour
{

  public LanguageSourceData LSData;
  public void Awake()
  {
    gameObject.name = "LocalizationManager";
    gameObject.hideFlags = HideFlags.HideAndDontSave;
  }
  public void Start()
  {
    GameObject resManGO = GameObject.Find("I2ResourceManager");
    if (resManGO != null)
    {
      ResourceManager res = resManGO.GetComponent<ResourceManager>();
      LanguageSourceAsset sourceAsset = res.mResourcesCache["I2Languages"].Cast<LanguageSourceAsset>();
      LSData = sourceAsset.SourceData;
      if (MissionControlPlugin.MCConf.autoLoadAssetPacks.Value)
      {
        ImportLocalization("Parts");
      }
    }
  }

  public void ImportLocalization(string category)
  {
    List<string> fileNames = Utils.Assets.GetFilesByExtension("csv");
    foreach (var fileName in fileNames)
    {
      string csvText = Utils.Assets.LoadCSV(fileName);
      if (csvText != null) LSData.Import_CSV(category, csvText, eSpreadsheetUpdateMode.AddNewTerms);
    }
  }
}