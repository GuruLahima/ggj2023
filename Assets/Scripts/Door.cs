using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
  [SerializeField] bool defaultState = false;
  [SerializeField] Sprite openState;
  [SerializeField] Sprite closedState;
  [SerializeField] SpriteRenderer _renderer;
  [SerializeField] BoxCollider2D _collider;
  [SerializeField] NavMeshObstacle _obstacle;

  private void Start()
  {
    SetState(defaultState);
  }
  public void SetState(bool open)
  {
    if (open)
    {
      _renderer.sprite = openState;
      _collider.enabled = false;
      _obstacle.enabled = false;
    }
    else
    {
      _renderer.sprite = closedState;
      _collider.enabled = true;
      _obstacle.enabled = true;
    }
  }
}
