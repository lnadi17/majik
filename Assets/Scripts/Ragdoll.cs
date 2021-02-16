using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private EnemyController enemyController;
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        SetKinematic(true);
        enemyController.DeathEvent += EnableRagdoll;
    }

    void SetKinematic(bool newValue) {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies) {
            rb.isKinematic = newValue;
        }
    }

    void EnableRagdoll() {
        SetKinematic(false);
    }

    private void OnDisable() {
        enemyController.DeathEvent -= EnableRagdoll;
    }
}
