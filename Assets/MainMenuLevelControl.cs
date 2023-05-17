using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLevelControl : MonoBehaviour
{
  [SerializeField] PortalInteraction motherPortal;
  [SerializeField] CutsceneData motherCompleted;
  [SerializeField] CutsceneData motherNotCompleted;
  [SerializeField] PortalInteraction grandfatherPortal;
  [SerializeField] CutsceneData grandfatherCompleted;
  [SerializeField] CutsceneData grandfatherNotCompleted;
  [SerializeField] PortalInteraction birdPortal;
  [SerializeField] CutsceneData birdCompleted;
  [SerializeField] CutsceneData birdNotCompleted;

  private void Start()
  {
    if (GameProgress.Instance)
    {
      if (GameProgress.Instance.BirdLevelPassed)
      {
        birdPortal.Cutscene = birdCompleted;
        birdPortal.IsCompleted = true;
      }
      else
      {
        birdPortal.Cutscene = birdNotCompleted;
        birdPortal.IsCompleted = false;
      }

      if (GameProgress.Instance.MotherFirstLevelPassed)
      {
        motherPortal.Cutscene = motherCompleted;
        motherPortal.IsCompleted = true;
      }
      else
      {
        motherPortal.Cutscene = motherNotCompleted;
        motherPortal.IsCompleted = false;
      }

      if (GameProgress.Instance.GrandpaLevelPassed)
      {
        grandfatherPortal.Cutscene = grandfatherCompleted;
        grandfatherPortal.IsCompleted = true;
      }
      else
      {
        grandfatherPortal.Cutscene = grandfatherNotCompleted;
        grandfatherPortal.IsCompleted = false;
      }
    }
  }
}
