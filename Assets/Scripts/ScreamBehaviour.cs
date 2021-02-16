using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScreamBehaviour : StateMachineBehaviour
{
    private NavMeshAgent agent;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        agent = animator.GetComponent<NavMeshAgent>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        agent.isStopped = true;
    }
}
