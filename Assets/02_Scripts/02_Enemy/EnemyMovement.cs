using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DestinationController))]

public class EnemyMovement : MonoBehaviour
{
    Transform targetPos;
    protected NavMeshAgent navAgent = null;
    [SerializeField]
    protected DestinationController destinationController;

    protected Animator animator;

    public Vector3 distination;

    [SerializeField]
    float battleRange;

    [SerializeField]
    float targetDistance;

    protected float timeleft;

    protected virtual void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        destinationController = GetComponent<DestinationController>();
        navAgent.SetDestination(destinationController.GetDestination());
        animator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        targetPos = player.transform;
    }

    protected virtual void Update()
    {

        // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
        distination = (targetPos.transform.position - transform.position).normalized;
        distination = new Vector3(distination.x, 0f, 0f).normalized;

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



        //移動先決定
        if (Vector3.Distance(transform.position, destinationController.GetDestination()) < 0.5f)
        {
            destinationController.CreateDestination();
            navAgent.SetDestination(destinationController.GetDestination());
        }

        //攻撃開始
        /*if (Vector3.Distance(transform.position, targetPos.position) < 0f)
        {
            Debug.Log(navAgent.remainingDistance);
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            {
                timeleft = 2.0f;
                attackIndex = Random.Range(1, 101);
                Debug.Log(attackIndex);

                if (1 <= attackIndex && attackIndex <= 21)
                {
                    animator.SetTrigger("H-Atk");
                }
                else if (31 <= attackIndex && attackIndex <= 51)
                {
                    animator.SetTrigger("Comb-Atk");
                }
                else
                {
                    animator.SetTrigger("L-Atk");
                }
            }
        }*/
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.tag == "AtkPoint")
        {
            Debug.Log("AtkPoint");
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            {
                timeleft = 2.0f;
                AttackIndex();

                if (1 <= attackIndex && attackIndex <= 21)
                {
                    animator.SetTrigger("H-Atk");
                }
                else if (31 <= attackIndex && attackIndex <= 51)
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
    //乱数用
    protected int attackIndex;

    /// <summary>
    /// 乱数生成＠攻撃アニメーションランダム再生
    /// </summary>
    protected void AttackIndex()
    {
        attackIndex = Random.Range(1, 101);
        //Debug.Log(attackIndex);
    }

    public void NaviMesh_OFF()
    {
        navAgent.isStopped = true;
    }

    public void NaviMesh_ON()
    {
        navAgent.isStopped = false;
    }
}
