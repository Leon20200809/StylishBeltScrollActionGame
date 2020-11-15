using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DestinationController))]

public class EnemyMovement_A : EnemyMovement
{
    
    protected override void OnTriggerStay(Collider other)
    {
        if (other.tag == "ShotAtkPoint")
        {
            Debug.Log("ShotAtkPoint");
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            {
                animator.SetTrigger("L-Atk");
            }

        }
    }
}
