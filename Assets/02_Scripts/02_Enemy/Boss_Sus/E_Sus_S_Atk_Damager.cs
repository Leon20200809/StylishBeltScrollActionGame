﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Sus_S_Atk_Damager : E_DamagerBase
{
    Collider collider;
    
    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("Player"))
        {
            collider = GetComponent<Collider>();
            Destroy(collider);
            //wcollider.enabled = false;
            Debug.Log(collider);
            Debug.Log("ボス必殺技HIT");
            //Destroy(this.gameObject);
        }
    }
}
