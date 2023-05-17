using NaughtyAttributes;
using UnityEngine;

public class GravityFieldAbility : Ability
{
  // this class makes the object collect rigidbody objects in a given radius
  // and applies a force to them in the direction of the object
  // the force is proportional to the distance from the object
  // the force is proportional to the mass of the object

  public float radius = 5f;
  public float force = 10f;
  public float mass = 1f;
  public LayerMask layerMask;
  public bool isPiggyBank;
  [ShowIf("isPiggyBank")]
  public float velocityThreshold;

  [SerializeField] Transform gravityPool;

  private void FixedUpdate()
  {
    if (MainCharacterController.gameIsPaused) return;
    if (!IsActive) return;
    var colliders = Physics2D.OverlapCircleAll(gravityPool.position, radius, layerMask);
    foreach (var collider in colliders)
    {
      if (isPiggyBank && collider.GetComponent<Rigidbody2D>() && collider.GetComponent<Rigidbody2D>().velocity.magnitude < velocityThreshold) continue;
      if (collider.gameObject == gameObject) continue;
      var rb = collider.GetComponent<Rigidbody2D>();
      if (rb == null) continue;
      var direction = (gravityPool.position - rb.transform.position).normalized;
      rb.AddForce(direction * force * rb.mass / mass);
    }
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, radius);
  }


}