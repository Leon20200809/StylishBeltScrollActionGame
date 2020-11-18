using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DestinationController))]

public class E_Skeleton_Movement : EnemyMovement
{
    protected override void OnTriggerStay(Collider other)
    {
        if (other.tag == "AtkPoint")
        {
            Debug.Log("AtkPoint");
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            {
                timeleft = 1.0f;
                AttackIndex();

                if (1 <= attackIndex && attackIndex <= 36)
                {
                    animator.SetTrigger("H-Atk");
                }
                else if (37 <= attackIndex && attackIndex <= 61)
                {
                    animator.SetTrigger("Comb-Atk");
                }
                else
                {
                    animator.SetTrigger("L-Atk");
                }
            }

        }

    }
}
