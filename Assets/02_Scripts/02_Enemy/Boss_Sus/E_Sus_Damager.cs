using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Sus_Damager : E_DamagerBase
{
    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("Player"))
        {
            wcollider.enabled = false;
            Debug.Log("敵ボスの攻撃HIT");
        }
    }

}
