using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSicknessAbility : Ability
{

  // this object takes damage when a coin is collected
  public float pushbackForce;

  public override void OnActivateAbility()
  {
  }

  public void TakeDamage()
  {
    // push player back when taking damage
    GetComponent<Rigidbody2D>().AddForce(Vector3.left * pushbackForce, ForceMode2D.Impulse);
  }

  public void OnCoinCollected()
  {
    TakeDamage();
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.transform.GetComponent<Projectile>())
    {
      OnCoinCollected();
      Destroy(other.gameObject);
    }
  }
}
