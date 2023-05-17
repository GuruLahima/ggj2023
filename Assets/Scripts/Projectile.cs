using UnityEngine;

public class Projectile : Detectable {
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 40;
    public GameObject impactEffect;

    void Start() {
        // rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo) {
        // Enemy enemy = hitInfo.GetComponent<Enemy>();
        // if (enemy != null) {
        //     enemy.TakeDamage(damage);
        // }

        // Instantiate(impactEffect, transform.position, transform.rotation);

        // Destroy(gameObject);
    }
  
}