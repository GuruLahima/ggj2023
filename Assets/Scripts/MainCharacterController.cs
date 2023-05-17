using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// 
/// </summary>
public class MainCharacterController : MonoBehaviour
{
  /// <summary>
  /// idea: ancestors can be ghosts that float around us. more ancestors you've beaten/helped more ghosts around you
  /// and you can switch between the ancestors with one button and use them with another (if their ability is triggerabe and not passive)
  /// so are constrained by only one ability at a time which gives much better gameplay since it's simpler, faster, and 
  /// makes sense with the narrative. like, you use a lesson from the past to overcome present obstacle.
  /// 
  /// possible abilities:
  /// - gravitational field (pulls objects towards you)
  /// - gravity (pulls you towards objects)
  /// - gravitational field 2 (you sling objects as they move past you)
  /// - objects move through you
  /// - super long jump (airborne for quite a while) (done)
  /// - reflect projectiles
  /// - swap places with objects (you are not damaged when swapping with said object)
  /// - 
  /// 
  /// </summary>
  public static bool gameIsPaused = false;

  #region references
  [Header("References")]
  public Animator anim;
  public SpriteRenderer playerSprite;
  // public Collider2D playerCollider;
  // public SpriteRenderer playerShadow;
  public Rigidbody2D rb;
  public GameObject pigeonOnHead;

  #endregion


  #region exposed fields
  [Header("Movement params")]
  [SerializeField] float speed = 10.0f;
  [SerializeField] float rotationSpeed = 100.0f;
  [SerializeField] float idleThreshold;

  #endregion


  #region private fields

  private IEnumerator jumpCoroutine = null;
  #endregion

  public void PauseGame(bool pause)
  {
    if (pause)
    {
      gameIsPaused = true;
    }
    else
    {
      gameIsPaused = false;
    }
  }

  private void Awake()
  {
    // unpause game at start of level
    Time.timeScale = 1.0f;
    MainCharacterController.gameIsPaused = false;
  }

  private void Update()
  {
    // if (MainCharacterController.gameIsPaused) { return; }
    // manage sprite animation
    if (rb.velocity.magnitude > idleThreshold)
    {
      // play walking animation
      anim.SetBool("isWalking", true);
      if (rb.velocity.x > 0)
      {
        // play walking right animation
        playerSprite.flipX = false;
      }
      else
      {
        // play walking left animation
        playerSprite.flipX = true;
      }
    }
    else
    {
      // play idle animation
      anim.SetBool("isWalking", false);
    }
  }


  void FixedUpdate()
  {
    if (MainCharacterController.gameIsPaused) { return; }
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");

    Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);

    rb.AddForce(movement * speed);

  }

}