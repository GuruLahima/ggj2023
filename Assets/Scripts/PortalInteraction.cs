using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PortalInteraction : Detectable
{
  [Scene]
  public string SceneToTeleportTo;
  public CutsceneData Cutscene;
  public bool IsCompleted;
}
