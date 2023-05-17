using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToggleStepping : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D collision)
  {
    var detectable = collision.GetComponent<Detectable>();
    if (detectable is TogglePlate togglePlate)
    {
      if (togglePlate)
      {
        togglePlate.ToggleState();
      }
    }
  }
}
