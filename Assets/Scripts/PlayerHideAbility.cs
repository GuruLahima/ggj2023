using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHideAbility : MonoBehaviour
{
  public List<Detectable> listOfHidingSpots = new List<Detectable>();

  [SerializeField] SpriteRenderer _renderer;
  [SerializeField] Color hiddenColor;
  [SerializeField] Color normalColor;
  [SerializeField] DetectableData hidingSpotsDetectable;

  private void Update()
  {
    if (listOfHidingSpots.Count > 0)
    {
      GetComponent<Detectable>().IsVisible = false;
      _renderer.color = normalColor;
    }
    else
    {
      GetComponent<Detectable>().IsVisible = true;
      _renderer.color = hiddenColor;
    }
  }


  private void OnTriggerEnter2D(Collider2D collision)
  {
    // Debug.Log("ENTERED COLLISION");
    var detectable = collision.GetComponent<Detectable>();
    if (detectable != null)
    {
      if (detectable.DetectableData == hidingSpotsDetectable)
      {
        if (!listOfHidingSpots.Contains(detectable))
        {
          listOfHidingSpots.Add(detectable);
        }
      }
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    // Debug.Log("LEFT COLLISION");
    var detectable = collision.GetComponent<Detectable>();
    if (detectable != null)
    {
      if (detectable.DetectableData == hidingSpotsDetectable)
      {
        if (listOfHidingSpots.Contains(detectable))
        {
          listOfHidingSpots.Remove(detectable);
        }
      }
    }
  }
}
