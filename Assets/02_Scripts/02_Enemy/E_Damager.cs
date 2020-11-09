using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Damager : MonoBehaviour
{
    public int atkPow_L;
    public int atkPow_H;
    public int atkPow_C;
    Collider wcollider;

    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("Player"))
        {
            
            wcollider.enabled = false;
            Debug.Log("敵の攻撃HIT");
            Debug.Log(wcollider);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        wcollider = GetComponent<Collider>();
        atkPow_L = GameData.instance.charaDataList[1].baseAttackPower;
        atkPow_H = atkPow_L * 3;
        atkPow_C = Mathf.RoundToInt(atkPow_L * 0.7f);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
