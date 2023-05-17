using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.U2D;
using System.Linq;
using System;
using UnityEngine.Events;

public class SlingshotAbility : Ability
{
  #region Game Events

  public GameEvent AttemptedGrabEvent;
  public GameEvent GrabbedObjectEvent;
  public GameEvent ReleasedObjectEvent;

  #endregion

  #region UnityEvents

  [SerializeField] UnityEvent OnAttemptedGrab;
  [SerializeField] UnityEvent OnGrabbedObject;
  [SerializeField] UnityEvent OnReleasedObject;

  #endregion

  #region public fields

  [Header("References")]
  public Collider2D playerCollider;
  public SpriteRenderer playerSprite;
  public SpriteRenderer playerShadow;
  public SpriteRenderer radiusSprite;
  public Rigidbody2D rb;
  #endregion


  #region exposed 
  [Header("params")]
  [SerializeField] float grabRadius = 2.0f;
  [SerializeField] float rotationRadius = 2.0f;
  [SerializeField] float releaseSpeedMod = 10f;
  [SerializeField] float rotationSpeed = 1f;

  [SerializeField] float elasticity;
  [SerializeField] bool automaticGrab;

  #endregion


  #region private fields
  // public Cinemachine.CinemachinePixelPerfect cinemachinePixelPerfectComponent;
  public PixelPerfectCamera pixelPerfectComponent;

  private IEnumerator abilityCoroutine = null;
  private Rigidbody2D grabbedObject;
  #endregion


  #region MonoBehaviour callbacks

  public override void OnActivateAbility()
  {
    base.OnActivateAbility();
    radiusSprite.transform.localScale = Vector3.one * grabRadius * 2;
    radiusSprite.enabled = true;
  }

  public override void OnDisableAbility()
  {
    base.OnDisableAbility();
    radiusSprite.enabled = false;
  }


  private void Update()
  {
    if (MainCharacterController.gameIsPaused) { return; }
    if (IsActive)
    {
      if (!automaticGrab)
      {
        if (Input.GetKeyDown(KeyCode.Space) && abilityCoroutine == null)
        {
          abilityCoroutine = GrabObject();
          StartCoroutine(abilityCoroutine);
        }
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (IsActive && automaticGrab && abilityCoroutine == null && other.GetComponent<Projectile>() != null)
    {
      abilityCoroutine = GrabObject();
      StartCoroutine(abilityCoroutine);
    }
  }
  #endregion


  #region public methods

  #endregion


  #region private methods
  private IEnumerator GrabObject()
  {
    Debug.Log("grabbing object");
    if (!automaticGrab)
    {
      OnAttemptedGrab?.Invoke();
      AttemptedGrabEvent?.Raise();
    }

    radiusSprite.GetComponent<Collider2D>().enabled = false;

    // get nearest projectile
    List<Collider2D> hits = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, grabRadius));
    // filter list to have only projectiles
    hits = hits.Where((obj) => obj.GetComponent<Projectile>() != null).ToList();
    Collider2D nearestObject = hits?.OrderBy((obj) => Vector2.Distance(obj.transform.position, transform.position)).FirstOrDefault();

    // create an anchor object to rotate around
    GameObject anchorObject = new GameObject();
    anchorObject.transform.position = transform.position;
    anchorObject.transform.parent = transform;

    IEnumerator keepAtDistanceCoroutine = null;

    // make the object rotate around the player at a certain radius
    if (nearestObject != null)
    {
      OnGrabbedObject?.Invoke();
      GrabbedObjectEvent?.Raise();

      grabbedObject = nearestObject.GetComponent<Rigidbody2D>();
      grabbedObject.GetComponent<Collider2D>().enabled = false;
      grabbedObject.velocity = Vector2.zero;
      grabbedObject.transform.parent = anchorObject.transform;
      grabbedObject.transform.rotation = Quaternion.identity;
      anchorObject.transform.DOLocalRotate(new Vector3(0, 0, 360), 1f / rotationSpeed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
      // grabbedObject.transform.DOLocalMove(new Vector3(0, radius, 0), 0.1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
      Vector3 localPos = (grabbedObject.transform.position - anchorObject.transform.position).normalized * rotationRadius;
      keepAtDistanceCoroutine = KeepAtDistance(grabbedObject.transform, anchorObject.transform, rotationRadius, elasticity);
      StartCoroutine(keepAtDistanceCoroutine);
    }


    if (grabbedObject != null)
    {
      yield return null;

      yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
      Debug.Log("releasing object");
      OnReleasedObject?.Invoke();
      ReleasedObjectEvent?.Raise();

      Invoke("RestoreTriggerCollider", 0.4f);

      // get the perpendicular 2d vector to the vector between the player and the anchor object
      Vector2 beforePos = grabbedObject.transform.position;
      yield return null;
      Vector2 afterPos = grabbedObject.transform.position;
      // float delta = Vector3.Distance(afterPos, beforePos);
      // float rotationalVelocity = delta / Time.deltaTime;
      Vector2 direction = (afterPos - beforePos).normalized;

      // shoot grabbed object like it was slingshot
      if (grabbedObject != null)
      {
        grabbedObject.transform.parent = null;
        grabbedObject.transform.DOKill();
        grabbedObject.velocity = direction * releaseSpeedMod/*  * rotationalVelocity */;
        grabbedObject.GetComponent<Collider2D>().enabled = true;

        grabbedObject = null;

      }

      if (grabbedObject != null)
      {
        grabbedObject.transform.parent = null;
        grabbedObject.transform.DOKill();
        grabbedObject = null;
      }

    }

    Destroy(anchorObject);

    StopCoroutine(abilityCoroutine);
    if (keepAtDistanceCoroutine != null)
      StopCoroutine(keepAtDistanceCoroutine);
    abilityCoroutine = null;
  }

  void RestoreTriggerCollider()
  {
    radiusSprite.GetComponent<Collider2D>().enabled = true;
    radiusSprite.color = new Color(radiusSprite.color.r, radiusSprite.color.g, radiusSprite.color.b, 1f);
  }

  private IEnumerator KeepAtDistance(Transform obj, Transform target, float distance, float speed)
  {
    Vector3 localPos = (obj.position - target.position).normalized * distance;
    while (true)
    {
      obj.transform.localPosition = Vector3.Lerp(obj.transform.localPosition, localPos, speed * Time.deltaTime);
      // obj.transform.localPosition = localPos;
      yield return null;
    }
  }

  private IEnumerator ReleaseObject(Rigidbody2D obj)
  {
    yield return null;
  }

  #endregion


  #region networking code

  #endregion

}
