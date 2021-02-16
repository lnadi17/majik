using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileController))]
public class ExplosiveArrowController : MonoBehaviour
{
    public int minDamage = 101;
    public int maxDamage = 101;
    public float explosionRadius = 10f;

    private bool hasCollided = false;

    private void OnTriggerEnter(Collider other) {
        // One projectile can give a damage once
        if (hasCollided) {
            return;
        } else {
            hasCollided = true;
        }

        // Implement projectile effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders) {
            // Basically checks if is colliding with character's hips
            EnemyController enemyController;
            if (col.transform.parent != null) {
                enemyController = col.transform.parent.GetComponent<EnemyController>();
            } else {
                continue;
            }
            if (enemyController != null) {
                enemyController.TakeDamage(Random.Range(minDamage, maxDamage));
            }
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb) {
                rb.AddExplosionForce(5000, transform.position, explosionRadius, 1000f);
            }
        }
        Destroy(this);
    }
}
