using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cutscene", menuName = "Data/CutsceneData")]
public class CutsceneData : ScriptableObject
{
  public List<CutsceneScenario> Scenarios = new List<CutsceneScenario>();
  public System.Action OnCutsceneStarted;
  public System.Action OnCutsceneEnded;
}

[System.Serializable]
public class CutsceneScenario
{
  public GameEvent OnScenarioStarted;
  public GameEvent OnScenarioEnded;
  [TextArea]
  public List<string> ListOfTexts = new List<string>();
  public ScenarioTextAllignment Allignment;
  [NaughtyAttributes.ShowAssetPreview]
  public Sprite LeftImage;
  [NaughtyAttributes.ShowAssetPreview]
  public Sprite RightImage;
  public string LeftName;
  public string RightName;
  public float talkingInterval;
  public bool waitForInput = true;
  public GameEvent waitForEvent;

  public enum ScenarioTextAllignment
  {
    Left,
    Right
  };
}