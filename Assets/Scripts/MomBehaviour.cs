using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MomBehaviour : MonoBehaviour
{
  #region Events

  public UnityEvent OnCollectedCoin;

  #endregion
  //
  [SerializeField] float followSpeed;
  [SerializeField] Transform player;
  [SerializeField] Transform endOfLevel;

  [SerializeField] Vector3 offset;
  float furthestXPos = 0;
  private IEnumerator followCoroutine;

  private void Start()
  {
    // offset = transform.position - player.position;
    offset.z = 0;
    furthestXPos = player.position.x;

    followCoroutine = FollowPlayer();
    StartCoroutine(followCoroutine);
  }

  private IEnumerator FollowPlayer()
  {
    while (true)
    {
      if (MainCharacterController.gameIsPaused)
      {
        yield return null;
        continue;
      }
      float x = player.position.x;
      if (furthestXPos < x)
      {
        furthestXPos = x;
      }
      Vector3 furthestPosition = new Vector3(furthestXPos, player.position.y, transform.position.z);
      furthestPosition.z = transform.position.z;
      transform.position = Vector3.Lerp(transform.position, furthestPosition + offset, followSpeed * Time.deltaTime);

      // if mom reached the end of the level
      if (transform.position.x > endOfLevel.position.x)
      {
        // player won
        StopFollowingPlayer();

        // todo: load next level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

      }
      yield return null;
    }

  }

  public void StopFollowingPlayer()
  {
    if (followCoroutine != null)
    {
      StopCoroutine(followCoroutine);
      //
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.transform.GetComponent<Projectile>() != null && other.enabled)
    {
      // mom collected a coin
      Destroy(other.gameObject);

      OnCollectedCoin?.Invoke();

    }
  }
}
