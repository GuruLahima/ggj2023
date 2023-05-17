using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
  PortalInteraction _currentPortal = null;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    var portal = collision.GetComponent<PortalInteraction>();
    if (portal)
    {
      _currentPortal = portal;
      if (_currentPortal != null)
      {
        if (_currentPortal.Cutscene != null)
        {
          CutsceneController.Instance.PlayCutscene(_currentPortal.Cutscene);
        }
      }
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    var portal = collision.GetComponent<PortalInteraction>();
    if (portal)
    {
      if (_currentPortal != null)
      {
        if (portal == _currentPortal)
        {
          CutsceneController.Instance.StopCutscenes();
          _currentPortal = null;
        }
      }

    }
  }

  private void Update()
  {
    if (_currentPortal != null)
    {
      if (Input.GetKeyDown(KeyCode.E) && !_currentPortal.IsCompleted)
      {
        LevelManager.Instance.LoadLevel(_currentPortal.SceneToTeleportTo);
      }
    }
  }
}
