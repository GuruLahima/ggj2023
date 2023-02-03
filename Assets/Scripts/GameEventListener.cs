using System;
using UnityEngine.Events;

[Serializable]
public class GameEventListener
{
  /* [ReadOnly] */ [UnityEngine.HideInInspector] public string name;
  public GameEvent Event;
  public UnityEvent Response;

  public void Register()
  { Event.RegisterListener(this); }

  public void Unregister()
  { Event.UnregisterListener(this); }

  public void OnEventRaised()
  { Response.Invoke(); }
}