using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
public class GameEventManager : MonoBehaviour
{
  [OnValueChanged("OnValueChangedCallback")]
  public List<GameEventListener> gameEventListeners = new List<GameEventListener>();

  private void OnEnable()
  {
    foreach (GameEventListener Event in gameEventListeners)
      Event.Register();
  }

  private void OnDisable()
  {
    foreach (GameEventListener Event in gameEventListeners)
      Event.Unregister();
  }


  private void OnValueChangedCallback()
  {
    gameEventListeners.ForEach((obj) => {obj.name = obj.Event.name;});

  }
}