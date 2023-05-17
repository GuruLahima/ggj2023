using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingDetectable : Detectable
{
  public string TextToBubble;
  public bool HideOnInteracted;
  public float TextDuration; // -1 == infinite
  public bool AutoDisableOnLeft = true; // this should never be false with infinite duration
  
}
