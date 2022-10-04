
using SimpleJSON;
using UnityEngine;

namespace MissionControl.Parts;

public class CustomWheelSO : WheelSO
{
  public CustomWheelSO(JSONNode wheelNode, WheelSO parent, GameObject prefab)
  {
    // PartSO
    this.title = wheelNode["title"];
    this.description = wheelNode["description"];
    this.name = wheelNode["name"];
    this.titleLocTerm = wheelNode["titleLocTerm"];
    this.titleTranslation = wheelNode["titleTranslation"];
    this.cost = wheelNode["cost"].AsInt;
    this.mass = wheelNode["mass"].AsInt;
    this.includeInDemo = wheelNode["includeInDemo"].AsBool;
    this.order = wheelNode["order"].AsInt;
    this.ID = wheelNode["ID"].AsInt;

    // Prefab
    this.prefab = prefab;

    // PartSO inherits 
    this.attachmentPoints = parent.attachmentPoints;
    this.meshGOPaths = parent.meshGOPaths;
    this.iconTex = parent.iconTex;
    this.actions = parent.actions;

    // WheelSO
    this.motorForce = wheelNode["motorForce"].AsFloat;
    this.throttleSpeed = wheelNode["throttleSpeed"].AsFloat;
    this.sideFrictionCoef = wheelNode["sideFrictionCoef"].AsFloat;
    this.slipForceBoost = wheelNode["slipForceBoost"].AsFloat;
    this.stickyForce = wheelNode["stickyForce"].AsFloat;
    this.wheelTreadFreq = wheelNode["wheelTreadFreq"].AsFloat;
    this.wheelTreadOffset = wheelNode["wheelTreadOffset"].AsFloat;
    this.wheelDamper = wheelNode["wheelDamper"].AsFloat;
    this.wheelSpring = wheelNode["wheelSpring"].AsFloat;
    this.applyForceRadius = wheelNode["applyForceRadius"].AsFloat;
    this.wheelWidth = wheelNode["wheelWidth"].AsFloat;
    this.wheelRadius = wheelNode["wheelRadius"].AsFloat;

    // WheelSO Inherits
    this.impactEventPath = parent.impactEventPath;
  }
}