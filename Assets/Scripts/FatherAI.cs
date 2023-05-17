using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FatherAI : BaseAI
{
  [SerializeField] float attackCooldown;
  [SerializeField] float offsetBeforeTarget;
  [SerializeField] float attackAcquisitionRange;
  [SerializeField] DetectableData hidingSpotDetectable;
  [SerializeField] DetectableData playerDetectable;
  [SerializeField] float hidingSpotCheckDuration;

  private Detectable currentTarget;
  private bool CanAttack => nextAttackTime < Time.time;
  float nextAttackTime;
  public override void TargetAI(Detectable target, TargetingAction action)
  {
    // need to prevent choosing a new target if we already have a target, unless the new target is a player?
    if (currentTarget)
    {
      if (currentTarget.DetectableData != playerDetectable && navMeshAgent.remainingDistance > offsetBeforeTarget)
      {
        return;
      }
    }
    Debug.Log("Father is targeting");
    if (action == TargetingAction.Attack)
    {
      if (CanAttack)
      {
        if (Vector2.Distance(target.transform.position, transform.position) <= attackAcquisitionRange)
        {
          AttackAI(target);
          nextAttackTime = attackCooldown + Time.time;
        }
        else
        {
          MoveToDestination(target.transform.position, attackAcquisitionRange);
        }
      }
      else
      {
        if (Vector2.Distance(target.transform.position, transform.position) <= attackAcquisitionRange)
        {
          // dont do anything, just wait for CD
        }
        else
        {
          MoveToDestination(target.transform.position, attackAcquisitionRange);
        }
      }
    }
    else if (action == TargetingAction.Follow)
    {
      if (target.DetectableData == hidingSpotDetectable && Vector2.Distance(target.transform.position, transform.position) <= offsetBeforeTarget)
      {
        StartCoroutine(CheckingHidingSpot(target));
      }
      else
      {
        MoveToDestination(target.transform.position, offsetBeforeTarget);
      }
    }
    else
    {
      WanderAI();
    }
  }

  IEnumerator CheckingHidingSpot(Detectable target)
  {
    target.IsVisible = false;
    yield return new WaitForSeconds(hidingSpotCheckDuration);
    target.IsVisible = true;
  }

  public override void AttackAI(Detectable target)
  {
    // do the attack here
    Debug.Log("Father is attacking");
  }
}
