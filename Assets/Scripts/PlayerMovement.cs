using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum CameraSettings { Follow, RotateAround };

public class PlayerMovement : MonoBehaviour
{
    public GameObject shootObject;
    public float shootStrength;

    [SerializeField]
    private float ikSnapSpeed;
    [SerializeField]
    private Rig aimRig;
    [SerializeField]
    private float aimRigActivationSpeed = 0.5f;
    [SerializeField]
    private Transform shootBase;

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (animator.GetBool("isAiming")) {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 1f, aimRigActivationSpeed * Time.deltaTime);
        } else {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0f, aimRigActivationSpeed * Time.deltaTime);
        }
    }

    private void OnAnimatorMove() {
        if (animator) {
            animator.ApplyBuiltinRootMotion();
        }
    }
}
