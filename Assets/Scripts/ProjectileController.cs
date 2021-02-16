using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileController : MonoBehaviour
{
    [Range(0, 1)]
    public float gravityScale;

    private float gravity = 9.8f;
    private Rigidbody rb;

    [SerializeField]
    private float rotationLerpSpeed = 20;

    private bool hasCollided = false;
    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (!hasCollided && rb.velocity != Vector3.zero)
            transform.forward = Vector3.Slerp(transform.forward, rb.velocity.normalized, rotationLerpSpeed * Time.deltaTime);
    }

    private void FixedUpdate() {
        if (!hasCollided && rb.velocity != Vector3.zero)
            rb.AddForce(gravity * gravityScale * Vector3.down, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other) {
        // Stop projectile
        if (!hasCollided) {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            hasCollided = true;
            Destroy(rb);
        }
    }

    public EnemyController FindEnemyControllerInParents(Transform t) {
        EnemyController controller = t.GetComponent<EnemyController>();
        if (controller == null && t.root != t) {
            return FindEnemyControllerInParents(t.parent);
        } else if (controller != null || t.root == t) {
            return controller;
        }
        return null;
    }
}
