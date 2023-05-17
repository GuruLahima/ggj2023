using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class Detectable : MonoBehaviour
{
  public DetectableData DetectableData;
  [Foldout("Events")]
  public UnityEvent OnBecameVisible;
  [Foldout("Events")]
  public UnityEvent OnBecameInvisible;
  public bool IsVisible
  {
    get
    {
      return _isVisible;
    }
    set
    {
      if (!_isVisible && value)
      {
        OnBecameVisible?.Invoke();
      }
      if (_isVisible && !value)
      {
        OnBecameInvisible?.Invoke();
      }
      _isVisible = value;
    }
  }

  [SerializeField] private bool _isVisible = true;
}
