using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerInput : MonoBehaviour
{
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetMouseButtonUp(0) && animator.GetBool("isAiming")) {
            animator.SetTrigger("Shoot");
        }

        if (Input.GetMouseButtonDown(1)) {
            animator.SetBool("isAiming", true);
        }

        if (Input.GetMouseButtonUp(1)) {
            animator.SetBool("isAiming", false);
        }

        if (Mathf.Abs(x) + Mathf.Abs(z) > Mathf.Epsilon) {
            animator.SetBool("isMoving", true);
            animator.SetFloat("ikWeight", 0);
        } else {
            animator.SetBool("isMoving", false);
            animator.SetFloat("ikWeight", Mathf.Lerp(animator.GetFloat("ikWeight"), 1f, 5f * Time.deltaTime));
        }

        animator.SetFloat("VelocityX", x);
        animator.SetFloat("VelocityZ", z);
    }
}
