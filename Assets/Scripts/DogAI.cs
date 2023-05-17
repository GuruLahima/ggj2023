using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAI : BaseAI
{
  [SerializeField] SpriteRenderer spriteRenderer;
  [SerializeField] DetectableData playerDetectable;
  [SerializeField] float playerMaxDistanceToFollow;
  [SerializeField] DetectableData dogCheckpointDetectable;
  [SerializeField] float playerStandDurationRequired;

  bool canInteractWithPlayer = false;

  private Vector3 _lastPlayerLocation;
  private float nextPlayerStandReady = float.PositiveInfinity;

  private void Start()
  {
    _lastPlayerLocation = LevelManager.Instance.GetDetectablesOfType(playerDetectable)[0].transform.position;
  }
  void FixedUpdate()
  {
    spriteRenderer.flipX = navMeshAgent.velocity.x > 0;
  }
  public override void TargetAI(Detectable target, TargetingAction action)
  {
    if (action == TargetingAction.Follow)
    {
      if (Vector2.Distance(LevelManager.Instance.GetDetectablesOfType(playerDetectable, false)[0].transform.position, transform.position) > playerMaxDistanceToFollow)
      {
        if (canInteractWithPlayer)
        {
          if (Vector3.Distance(_lastPlayerLocation, LevelManager.Instance.GetDetectablesOfType(playerDetectable)[0].transform.position) < 0.1f)
          {
            if (nextPlayerStandReady < Time.time)
            {
              targetingPriorities[0].Enabled = false;
              targetingPriorities[1].Enabled = true;
              playerMaxDistanceToFollow = float.PositiveInfinity;
            }
          }
          else
          {
            nextPlayerStandReady = playerStandDurationRequired + Time.time;
          }
        }
        _lastPlayerLocation = LevelManager.Instance.GetDetectablesOfType(playerDetectable)[0].transform.position;
        MoveToDestination(transform.position, 0f);
      }
      else
      {
        // we want the dog to stop exactly there
        if (Vector2.Distance(target.transform.position, transform.position) < 0.3f)
        {
          // if we arrived to that target, disable and continue to next one
          target.IsVisible = false;
        }
        MoveToDestination(target.transform.position, 0f);
      }
    }
  }

  public override void WanderAI()
  {
    canInteractWithPlayer = true;
    foreach (Detectable det in LevelManager.Instance.GetDetectablesOfType(dogCheckpointDetectable, false))
    {
      det.IsVisible = true;
    }
    // here we reset all dog checkpoints

  }
}
