using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileController))]
public class ArrowController : MonoBehaviour
{
    public float hitForce = 150;
    public int minDamage = 10;
    public int maxDamage = 70;

    private bool hasCollided = false;

    private void OnTriggerEnter(Collider other) {
        // One projectile can give a damage once
        if (hasCollided) {
            return;
        } else {
            hasCollided = true;
        }

        // Implement projectile effect
        if (other.gameObject.layer == LayerMask.NameToLayer("Character")) {
            EnemyController enemyController = FindEnemyControllerInParents(other.transform);
            if (enemyController != null) {
                enemyController.TakeDamage(Random.Range(minDamage, maxDamage));
            }
        }

        Rigidbody rbOther = other.GetComponent<Rigidbody>();
        if (rbOther) {
            rbOther.AddForce(hitForce * transform.forward, ForceMode.Impulse);
            transform.parent = rbOther.transform;
        }

        Destroy(this);
    }

    private EnemyController FindEnemyControllerInParents(Transform t) {
        EnemyController controller = t.GetComponent<EnemyController>();
        if (controller == null && t.root != t) {
            return FindEnemyControllerInParents(t.parent);
        } else if (controller != null || t.root == t) {
            return controller;
        }
        return null;
    }
}
