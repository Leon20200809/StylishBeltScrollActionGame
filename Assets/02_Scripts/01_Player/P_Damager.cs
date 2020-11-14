using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_Damager : MonoBehaviour
{
    public int atkPow_L;
    public int atkPow_Kick;
    public int atkPow_H;
    public int atkPow_I;
    public int atkPow_Z;
    public int atkPow_Kumiuchi;
    public int atkPow_Oiuchi;
    Collider wcollider;
    public PlayerController playerController;

    public int hitCount;
    public float comboTime;
    public Text combCountText;


    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("Enemy") || other.CompareTag("BossEnemy"))
        {
            wcollider.enabled = false;
            ComboCountStart();
            Debug.Log("プレイヤー攻撃HIT");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        wcollider = GetComponent<Collider>();
        atkPow_L = GameData.instance.charaDataList[0].baseAttackPower;
        atkPow_Kick = Mathf.RoundToInt(atkPow_L * 0.3f);
        atkPow_H = Mathf.RoundToInt(atkPow_L * 1.4f);
        atkPow_I = atkPow_L * 3;
        atkPow_Z = Mathf.RoundToInt(atkPow_L * 1.3f);
        atkPow_Kumiuchi = atkPow_L * 7;
        atkPow_Oiuchi = atkPow_L * 3;
    }

    // Update is called once per frame
    void Update()
    {
        comboTime += Time.deltaTime;
        if (comboTime > 2f)
        {
            comboTime = 0f;
            hitCount = 0;
        }
        combCountText.text = hitCount.ToString();
    }


    public void ComboCountStart()
    {
        hitCount++;
        comboTime = 0;
    }


}
