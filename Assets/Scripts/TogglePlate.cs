using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlate : Detectable
{
  [SerializeField] SpriteRenderer _renderer;
  [SerializeField] Sprite toggledOn;
  [SerializeField] Sprite toggledOff;
  public void ToggleState()
  {
    IsVisible = !IsVisible;
    if (IsVisible)
    {
      _renderer.sprite = toggledOn;
    }
    else
    {
      _renderer.sprite = toggledOff;
    }
  }
}
