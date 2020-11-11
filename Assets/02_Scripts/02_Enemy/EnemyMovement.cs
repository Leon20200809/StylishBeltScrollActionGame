using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DestinationController))]

public class EnemyMovement : MonoBehaviour
{
    public Transform targetPos;
    private NavMeshAgent navAgent = null;
    [SerializeField]
    private DestinationController destinationController;

    Animator animator;

    [SerializeField]
    Vector3 distination;


    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        destinationController = GetComponent<DestinationController>();
        navAgent.SetDestination(destinationController.GetDestination());
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
        Vector3 distination = (targetPos.transform.position - transform.position).normalized;
        distination = new Vector3(distination.x, 0f, 0f).normalized;
        Debug.Log(distination);

        if (distination.x == 1)
        {
            transform.LookAt(targetPos);
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

        }
        else
        {
            transform.LookAt(targetPos);
            transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

        }
        animator.SetFloat("Speed", navAgent.velocity.magnitude);



        if (Vector3.Distance(transform.position, destinationController.GetDestination()) < 0.5f)
        {
            destinationController.CreateDestination();
            navAgent.SetDestination(destinationController.GetDestination());
        }
        else if (Vector3.Distance(transform.position, destinationController.GetDestination()) < 1f)
        {
            //navAgent.speed = 0f;

            if (attackDice > 1f)
            {
                Debug.Log("攻撃");
            }
        }


    }

    //乱数用
    float attackDice;

    /// <summary>
    /// 乱数生成＠攻撃アニメーションランダム再生
    /// </summary>
    public void RandomAttackIndex()
    {
        attackDice = Random.value;
        animator.SetFloat("RandomAttackIndex", attackDice);
        //Debug.Log("乱数 : " + randomAttack);
    }
}
