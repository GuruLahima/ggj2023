using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CursorOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  public UnityEvent OnPointerEnteredUI;
  public UnityEvent OnPointerExitedUI;



  public void OnPointerEnter(PointerEventData eventData)
  {
    OnPointerEnteredUI?.Invoke();
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    OnPointerExitedUI?.Invoke();
  }
}
