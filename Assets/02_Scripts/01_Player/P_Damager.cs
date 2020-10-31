using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Damager : MonoBehaviour
{
    Collider wcollider;
    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("Enemy") || other.CompareTag("BossEnemy"))
        {
            wcollider.enabled = false;
            Debug.Log("プレイヤー攻撃ＨＩＴ");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        wcollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
