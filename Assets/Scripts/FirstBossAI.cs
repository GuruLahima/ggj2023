using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FirstBossAI : BaseAI
{
  public UnityEvent CollectedCoin;
  [SerializeField] DetectableData coinProjectileDetectable;
  [SerializeField] float m_speedBoostPerCoin;
  [SerializeField] float m_maximumSpeed;
  [SerializeField] GameObject gameOverScreen;
  [SerializeField] DetectableData playerDetectable;

  public override void TargetAI(Detectable target, TargetingAction action)
  {
    if (!MainCharacterController.gameIsPaused)
      MoveToDestination(target.transform.position, 0f);
    else
    {
      navMeshAgent.isStopped = true;
    }

  }

  public void Pause()
  {
    navMeshAgent.isStopped = true;
  }

  public void UnPause()
  {
    navMeshAgent.isStopped = false;
  }


  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (MainCharacterController.gameIsPaused) return;

    Debug.Log("boss collided");
    var detectable = collision.gameObject.GetComponent<Detectable>();
    if (detectable.DetectableData == coinProjectileDetectable)
    {
      CollectedCoin?.Invoke();
      TriggerSpeedBoost();
      // permanently destroy the projectile hit
      Destroy(detectable.gameObject);
    }

    // if boss touched player game restarts
    if (detectable.DetectableData == playerDetectable)
    {
      Debug.Log("boss touched player");

      // todo: restart level
      // pause game
      // Time.timeScale = 0;
      MainCharacterController.gameIsPaused = true;


      // show game over screen
      // gameOverScreen.SetActive(true);

      // pressing space restarts level



    }
  }

  private void TriggerSpeedBoost()
  {
    navMeshAgent.speed += m_speedBoostPerCoin;
    if (navMeshAgent.speed > m_maximumSpeed)
    {
      navMeshAgent.speed = m_maximumSpeed;
    }
  }

}
