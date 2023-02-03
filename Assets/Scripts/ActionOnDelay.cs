using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Workbench.Wolfsbane.Multiplayer
{

  public class ActionOnDelay : MonoBehaviour
  {
    public float delay;
    public float delayForOnEnable;
    [Tooltip("Action to trigger only once (after delay amount of time")] public UnityEvent ActionToTriggerOnDelay;
    [Tooltip("Action to trigger on every enable of this component (after delayForOnEnable amount of time")] public UnityEvent ActionToTriggerOnEnable;

    void Start()
    {
      Invoke("InvokeDelayedACtion", delay);
    }

    private void OnEnable()
    {
      Invoke("InvokeDelayedActionOnEnable", delayForOnEnable);
    }

    void InvokeDelayedACtion()
    {
      ActionToTriggerOnDelay?.Invoke();
    }
    void InvokeDelayedActionOnEnable()
    {
      ActionToTriggerOnEnable?.Invoke();
    }

  }
}