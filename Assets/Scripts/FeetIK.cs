using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FeetIK : MonoBehaviour
{
    public LayerMask groundMask;
    public float distanceToGround;
    public float raycastHeight;
    public Transform leftFoot;
    public Transform rightFoot;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float weight = animator.GetFloat("ikWeight");
        if (animator.GetBool("isMoving"))
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weight);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weight);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weight);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weight);

            // First we get the normals of the surfaces for both feet
            Vector3 normalForLeft = Vector3.zero;
            Vector3 normalForRight = Vector3.zero;

            RaycastHit hit;
            Ray ray;

            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, 3f, groundMask))
            {
                normalForLeft = hit.normal;
            }

            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, 3f, groundMask))
            {
                normalForRight = hit.normal;
            }

            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up * raycastHeight, -normalForLeft);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, groundMask))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += distanceToGround;
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.FromToRotation(Vector3.up, normalForLeft) * leftFoot.rotation);
            }

            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up * raycastHeight, -normalForRight);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, groundMask))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += distanceToGround;
                animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.FromToRotation(Vector3.up, normalForRight) * rightFoot.rotation);
            }
        }
    }
}
