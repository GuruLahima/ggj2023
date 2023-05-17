using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PigeonAI : BaseAI
{
  [SerializeField] Transform target;
  [SerializeField] float size;
  [Range(0f, 100f)]
  [SerializeField] float chanceToLandOnPlayer;
  [SerializeField] SpriteRenderer pigeonSprite;
  [SerializeField] float pigeonOnHeadDuration;
  [SerializeField] float pigeonSpeedWhenLanding;
  [SerializeField] float pigeonSpeedNormal;

  bool landingOnPlayer = false;

  public override void ThinkAI()
  {
    if (landingOnPlayer)
    {
      return;
    }
    if (Random.Range(0f, 100f) < chanceToLandOnPlayer)
    {
      LandOnPlayer();
    }
    else
    {
      MoveToDestination(FindRandomPositionIn(), 0f);
    }
  }

  private Vector2 FindRandomPositionIn()
  {
    Vector2 pos = new Vector2(Random.Range(target.position.x - size, target.position.x + size), Random.Range(target.position.y - size, target.position.y + size));
    return pos;
  }

  private void LandOnPlayer()
  {
    StartCoroutine(LandOnPlayerSequence());
  }

  IEnumerator LandOnPlayerSequence()
  {
    landingOnPlayer = true;
    navMeshAgent.speed = pigeonSpeedWhenLanding;
    while (Vector2.Distance(transform.position, target.position) > 0.2f)
    {
      MoveToDestination(target.transform.position, 0f);
      yield return null;
    }
    var cc = target.gameObject.GetComponent<MainCharacterController>();
    if (cc)
    {
      cc.pigeonOnHead.SetActive(true);
    }
    pigeonSprite.enabled = false;
    yield return new WaitForSeconds(pigeonOnHeadDuration);
    if (cc)
    {
      cc.pigeonOnHead.SetActive(false);
    }
    navMeshAgent.speed = pigeonSpeedNormal;
    transform.position = target.transform.position;
    pigeonSprite.enabled = true;
    landingOnPlayer = false;

  }



}
