using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DestinationController : MonoBehaviour
{
    //初期位置
    private Vector3 startPosition;
    //目的地
    [SerializeField] private Vector3 destination;

    [SerializeField] private Transform[] targets;

    [SerializeField] private int order = 0;

    public enum Route { inOrder, random }
    public Route route;

    public enum EnemyType
    {
        Sword_Soldier,
        Archer_Soldier,
        Boss,
    }

    public EnemyType enemyType;

    public void Select_Enemy_Type()
    {
        switch (enemyType)
        {
            case EnemyType.Sword_Soldier:

                targets = GameObject.FindGameObjectsWithTag("AtkPoint").Select(x => x.transform).ToArray();
                Debug.Log(targets[0].transform.position);

                break;

            case EnemyType.Archer_Soldier:

                targets = GameObject.FindGameObjectsWithTag("ShotPoint").Select(x => x.transform).ToArray();
                Debug.Log(targets[0].transform.position);

                break;
            case EnemyType.Boss:
                //処理
                GameObject[] atkPoints = GameObject.FindGameObjectsWithTag("AtkPoint");
                GameObject[] shotPoints = GameObject.FindGameObjectsWithTag("ShotPoint");

                //リスト作成。コンストラクタによる代入
                List<GameObject> tagObjs = new List<GameObject>(atkPoints);

                for (int i = 0; i< shotPoints.Length; i++)
                {
                    tagObjs.Add(shotPoints[i]);
                }
                targets = tagObjs.Select(x => x.transform).ToArray();
                break;
        }
    }


    void Start()
    {
        Select_Enemy_Type();
        //targets = GameObject.FindGameObjectsWithTag("AtkPoint").Select(x => x.transform).ToArray();
        //Debug.Log(targets[0].transform.position);
        //　初期位置を設定
        startPosition = transform.position;
        SetDestination(transform.position);
    }

    public void CreateDestination()
    {
        if (route == Route.inOrder)
        {
            CreateInOrderDestination();
        }
        else if (route == Route.random)
        {
            CreateRandomDestination();
        }
    }

    //targetsに設定した順番に作成
    private void CreateInOrderDestination()
    {
        if (order < targets.Length - 1)
        {
            order++;
            SetDestination(new Vector3(targets[order].transform.position.x, 0, targets[order].transform.position.z));
        }
        else
        {
            order = 0;
            SetDestination(new Vector3(targets[order].transform.position.x, 0, targets[order].transform.position.z));
        }
    }

    //　targetsからランダムに作成
    private void CreateRandomDestination()
    {
        int num = Random.Range(0, targets.Length);
        SetDestination(new Vector3(targets[num].transform.position.x, 0, targets[num].transform.position.z));
    }

    //　目的地の設定
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //　目的地の取得
    public Vector3 GetDestination()
    {
        return destination;
    }

}
