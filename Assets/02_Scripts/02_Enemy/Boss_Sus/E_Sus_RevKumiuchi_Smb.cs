using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Sus_RevKumiuchi_Smb : StateMachineBehaviour
{
    E_Sus_RevAtk kumiuchiCol;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // スクリプトを取得していない場合には取得する
        if (kumiuchiCol == null)
        {
            kumiuchiCol = animator.gameObject.GetComponent<E_Sus_RevAtk>();
            Debug.Log(kumiuchiCol);
        }
        kumiuchiCol.KumiuchiColON();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        kumiuchiCol.KumiuchiColOFF();
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
