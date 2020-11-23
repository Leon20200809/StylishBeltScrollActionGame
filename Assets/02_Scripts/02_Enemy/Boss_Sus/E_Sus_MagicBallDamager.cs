using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Sus_MagicBallDamager : E_DamagerBase
{
    protected override void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("Player"))
        {
            Debug.Log("マジックボールHIT");
            Destroy(this.gameObject);
        }
    }
}
