using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerTest_A : MonoBehaviour
{
    Collider wcollider;
    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("Enemy") || other.CompareTag("BossEnemy"))
        {
            Debug.Log("弱攻撃ＨＩＴ");
            wcollider.enabled = false;
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
