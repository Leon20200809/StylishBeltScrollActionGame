using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_DamagerBase : MonoBehaviour
{
    //敵の通し番号
    public int EnemyIndex;
    public int atttackPowerBase;

    protected Collider wcollider;


    public int atkPow_L;
    public int atkPow_H;
    public int atkPow_C;
    public int atkPow_M;
    public int atkPow_S;

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        Debug.Log(other.gameObject.layer);

        //タグ判定
        if (other.CompareTag("Player"))
        {
            wcollider.enabled = false;
            Attack_Successful();
        }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        wcollider = GetComponent<Collider>();
        atttackPowerBase = GameData.instance.charaDataList[EnemyIndex].baseAttackPower;

        atkPow_L = atttackPowerBase;
        atkPow_H = atkPow_L * 3;
        atkPow_C = Mathf.RoundToInt(atkPow_L * 0.7f);
        atkPow_M = atttackPowerBase;
        atkPow_S = atttackPowerBase * 5;
    }

    protected virtual void Attack_Successful()
    {
        Debug.Log(wcollider.enabled);
        Debug.Log("敵の攻撃HIT");
    }
}
