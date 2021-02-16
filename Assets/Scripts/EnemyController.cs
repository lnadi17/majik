using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Transform goal;
    public float lookRadius = 10f;
    public int health = 100;
    public int damage = 5;

    [Range(0, 1)]
    public float screamChance = 0.5f;

    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private float facePlayerSpeed = 10f;

    public delegate void OnDeathDelegate();
    public event OnDeathDelegate DeathEvent;

    private NavMeshAgent agent;
    private Animator animator;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthBar.maxValue = health;
        healthBar.value = healthBar.maxValue;
        SetKinematic(true);
        DeathEvent += Death;
    }

    private void Update() {
        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);

        if (InAttackRange()) {
            FaceTarget();
            animator.SetBool("isAttacking", true);
        }
        if (InLookRange()) {
            agent.destination = goal.position;
            agent.isStopped = false;
        } else {
            agent.isStopped = true;
        }
    }

    private bool InLookRange() {
        float distance = Vector3.Distance(goal.position, transform.position);
        return distance <= lookRadius;
    }

    private bool InAttackRange() {
        float distance = Vector3.Distance(goal.position, transform.position);
        return distance <= agent.stoppingDistance;
    }

    private void FaceTarget() {
        Vector3 direction = (goal.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * facePlayerSpeed);
    }

    // Method only for debugging
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.blue;
        if (agent)
            Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);
    }

    private void EnemyAttack() {
        // Do a primitive check to know if a player is in the vicinity
        Collider[] overlaps = Physics.OverlapSphere(transform.position, agent.stoppingDistance);
        foreach (Collider col in overlaps) {
            if (col.CompareTag("Player")) {
                col.GetComponentInParent<PlayerHealth>().TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damage) {
        // There's a [screamChance] chance that enemy is going to scream from pain
        Scream(screamChance);
        health -= damage;
        healthBar.value -= damage;
        if (health <= 0) {
            // Allow events to fire only once after dying
            if (!this.enabled) {
                return;
            }
            DeathEvent?.Invoke();
            this.enabled = false;
        }
    }

    void Death() {
        healthBar.value = healthBar.minValue;
        agent.enabled = false;
        animator.enabled = false;
    }

    void Scream(float chance) {
        if (Random.Range(0, 1f) > chance) {
            if (animator) {
                animator.SetTrigger("screamTrigger");
            }
        }
    }

        // This method is not needed if tuned from inspector, it just needs turning some checkboxes off
    void SetKinematic(bool newValue) {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies) {
            rb.isKinematic = newValue;
        }
    }

    private void OnDisable() {
        DeathEvent -= Death;
    }
}
