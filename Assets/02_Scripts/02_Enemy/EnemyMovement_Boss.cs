using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DestinationController))]

public class EnemyMovement_Boss : EnemyMovement
{

    protected override void OnTriggerStay(Collider other)
    {
        if (other.tag == "ShotAtkPoint")
        {
            Debug.Log("ShotAtkPoint");
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            {
                timeleft = 2.0f;
                AttackIndex();

                if (1 <= attackIndex && attackIndex <= 31)
                {
                    animator.SetTrigger("S-Atk");
                }
                else
                {
                    animator.SetTrigger("M-Atk");
                }

            }
        }

        if (other.tag == "AtkPoint")
        {
            Debug.Log("AtkPoint");
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            {
                timeleft = 2.0f;
                AttackIndex();

                if (1 <= attackIndex && attackIndex <= 26)
                {
                    animator.SetTrigger("H-Atk");
                }
                else if (36 <= attackIndex && attackIndex <= 51)
                {
                    animator.SetTrigger("S-Atk");
                }
                else
                {
                    animator.SetTrigger("L-Atk");
                }
            }

        }
    }
}
