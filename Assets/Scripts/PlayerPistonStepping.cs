using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPistonStepping : MonoBehaviour
{
  [SerializeField] DetectableData pistonDetectable;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    var detectable = collision.GetComponent<Detectable>();
    if (detectable)
    {
      if (detectable.DetectableData == pistonDetectable)
      {
        detectable.IsVisible = false;
      }  
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    var detectable = collision.GetComponent<Detectable>();
    if (detectable)
    {
      if (detectable.DetectableData == pistonDetectable)
      {
        detectable.IsVisible = true;
      }
    }
  }
}
