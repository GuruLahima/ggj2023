using System;
using NaughtyAttributes;
using UnityEngine;

public class Ability : MonoBehaviour
{
  public bool isActive = false;
  public bool IsActive
  {
    get
    {
      return isActive;
    }
    set
    {
      isActive = value;
      if (isActive)
        OnActivateAbility();
      else
        OnDisableAbility();
    }
  }

  [Button("Activate Ability")]
  void ActivateAbility()
  {
    IsActive = true;
  }
  [Button("Disable Ability")]
  void DisableAbility()
  {
    IsActive = false;
  }

  public virtual void OnEnable()
  {
    if (isActive)
      OnActivateAbility();
    else
      OnDisableAbility();
  }


  public virtual void OnActivateAbility()
  {

  }

  public virtual void OnDisableAbility()
  {

  }
}