using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BaseAI : MonoBehaviour
{
  // property that returns if thinking is ready
  private bool CanThink => nextThinkInterval < Time.time && !isStunned;


  [SerializeField] protected NavMeshAgent navMeshAgent;
  [SerializeField] float thinkInterval;
  [SerializeField] protected List<TargetingPriorityAI> targetingPriorities = new List<TargetingPriorityAI>();
  [SerializeField] List<ActionStateAI> actionStates = new List<ActionStateAI>();
  [SerializeField] ActionStateAI defaultAction;

  // cd for thinking
  private float nextThinkInterval = 0;
  private bool isStunned = false;

  private void Awake()
  {
    navMeshAgent.updateUpAxis = false;
    navMeshAgent.updateRotation = false;
  }

  private void Update()
  {
    if(MainCharacterController.gameIsPaused)
    {
      return;
    }
    if (CanThink)
    {
      nextThinkInterval = thinkInterval + Time.time;
      ThinkAI();
    }

    var currentState = actionStates.Find((x) => x.IsActive);
    if (currentState != null)
    {
      currentState.StateActions.Invoke();
    }
    else
    {
      defaultAction.StateActions?.Invoke();
    }
  }

  public virtual void ThinkAI()
  {
    Debug.Log("Thinking");
    for (int i = 0; i < targetingPriorities.Count; i++)
    {
      if (!targetingPriorities[i].Enabled)
      {
        // if this targeting option is disabled, continue to next iteration
        continue;
      }
      var t = LevelManager.Instance.GetDetectablesOfType(targetingPriorities[i].Target, targetingPriorities[i].FilterVisible);
      if (t.Count > 0)
      {
        if (t.Count == 1 || targetingPriorities[i].Choice == TargetingChoice.First)
        {
          TargetAI(t[0], targetingPriorities[i].Action);
          return; // prevent future iteration
        }
        else
        {
          TargetAI(FilterTargetByChoice(t, targetingPriorities[i].Choice), targetingPriorities[i].Action);
          return; // prevent future iteration
        }
      }
    }
    // if all cases fail do default action
    WanderAI();
    return;
  }

  // virtual so you can choose what to do with target in child scripts
  public virtual void TargetAI(Detectable target, TargetingAction action)
  {
    // what to do when you have target??
  }


  // override this method in child scripts
  public virtual void MoveAI(Detectable target)
  {

  }

  public virtual void AttackAI(Detectable target)
  {

  }

  // what should the AI do when no target found
  public virtual void WanderAI()
  {

  }

  protected Detectable FilterTargetByChoice(List<Detectable> dets, TargetingChoice choice)
  {
    return choice switch
    {
      TargetingChoice.Closest => FindClosestTarget(dets),
      TargetingChoice.Furthest => FindFurthestTarget(dets),
      TargetingChoice.Random => dets[Random.Range(0, dets.Count)],
      TargetingChoice.First => dets[0],
      _ => dets[0],
    };
  }

  protected Detectable FindClosestTarget(List<Detectable> dets)
  {
    int indexOfClosest = 0;
    float distanceOfClosest = float.PositiveInfinity;
    for (int i = 0; i < dets.Count; i++)
    {
      float dist = Vector2.Distance(dets[i].transform.position, transform.position);
      if (dist < distanceOfClosest)
      {
        distanceOfClosest = dist;
        indexOfClosest = i;
        continue;
      }
    }
    return dets[indexOfClosest];
  }

  protected Detectable FindFurthestTarget(List<Detectable> dets)
  {
    int indexOfFurthest = 0;
    float distanceOfFurthest = float.NegativeInfinity;
    for (int i = 0; i < dets.Count; i++)
    {
      float dist = Vector2.Distance(dets[i].transform.position, transform.position);
      if (dist > distanceOfFurthest)
      {
        distanceOfFurthest = dist;
        indexOfFurthest = i;
        continue;
      }
    }
    return dets[indexOfFurthest];
  }

  // returns true if the state was changed successfuly
  protected bool SetState(string stateToActivate)
  {
    var state = actionStates.Find((x) => x.StateName == stateToActivate);
    if (state != null)
    {
      foreach (ActionStateAI st in actionStates)
      {
        state.IsActive = st.StateName == stateToActivate;
      }
      // state successfuly activated
      return true;
    }
    else
    {
      // state has not been changed
      return false;
    }
  }

  // returns true if the destination was set successfuly
  protected bool MoveToDestination(Vector2 destination, float acquisitionRange)
  {
    // Debug.Log("Moving to destination " + destination);
    bool pass = navMeshAgent.SetDestination(destination);
    if (pass) navMeshAgent.stoppingDistance = acquisitionRange;
    return pass;
  }
  // think where to move

  // target priorty?

  // find transform for target
}

[System.Serializable]
public class TargetingPriorityAI
{
  public DetectableData Target;
  public TargetingChoice Choice;
  public TargetingAction Action;
  public bool FilterVisible;
  public bool Enabled = true;
  // condition??
  // priority based on list member??
}

[System.Serializable]
public class ActionStateAI
{
  // this is pretty much all you need, just call functions through events 
  public string StateName;
  public UnityEvent StateActions;
  [HideInInspector] public bool IsActive = false;
}

public enum TargetingChoice
{
  Closest = 0,
  Furthest = 1,
  Random = 2,
  First = 3
};

public enum TargetingAction
{
  Follow = 0,
  Attack = 1
};