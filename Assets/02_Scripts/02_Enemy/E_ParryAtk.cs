using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_ParryAtk : StateMachineBehaviour
{
    E_Input enemyInput;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // スクリプトを取得していない場合には取得する
        if (enemyInput == null)
        {
            enemyInput = animator.gameObject.GetComponent<E_Input>();
        }
        enemyInput.e_Weaponcollider.tag = "E_ParryAtk";
        enemyInput.GenerateEffect(animator.gameObject);
        animator.ResetTrigger("Rev-Atk");
        animator.ResetTrigger("Rev-Down");
        enemyInput.e_Parrycollider.enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyInput.e_Weaponcollider.tag = "E_Weapon";
        animator.ResetTrigger("Rev-Atk");
        animator.ResetTrigger("Rev-Down");
        enemyInput.e_Parrycollider.enabled = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
