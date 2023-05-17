using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
  private List<GameEventListener> listeners =
    new List<GameEventListener>();
  private bool isRaised;

  public void Raise()
  {
    isRaised = true;
    EmptyClass coroutineSurrogate = new GameObject().AddComponent<EmptyClass>();
    coroutineSurrogate.StartCoroutine(ResetRaised());
    for (int i = listeners.Count - 1; i >= 0; i--)
      listeners[i].OnEventRaised();
  }

  private IEnumerator ResetRaised()
  {
    yield return null;
    isRaised = false;
  }

  public void RegisterListener(GameEventListener listener)
  { listeners.Add(listener); }

  public void UnregisterListener(GameEventListener listener)
  { listeners.Remove(listener); }

  public bool WasReaisedThisFrame()
  {
    return isRaised;
  }
}