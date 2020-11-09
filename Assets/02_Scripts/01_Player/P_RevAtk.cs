﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class P_RevAtk : MonoBehaviour
{
    bool isDead;
    public int maxHp;
    public int hp;
    PlayerController playerController;
    Animator animator;
    GameManager gameManager;
    Rigidbody rb;
    [SerializeField]
    Vector3 distination;

    

    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
        {
            return;
        }
        //ダメージソース取得
        other.gameObject.TryGetComponent(out E_Damager damager);

        // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
        Vector3 distination = (other.transform.position - transform.position).normalized;
        distination = new Vector3(distination.x, 0f, 0f).normalized;

        //タグ判定
        if (other.CompareTag("E_Weapon"))
        {
            Damage(damager.atkPow_L);
            animator.SetTrigger("Rev-Atk");
            GenerateEffect(other.gameObject);
            Debug.Log("のけぞり小");
        }
        else if (other.CompareTag("E_ParryAtk"))
        {
            Damage(damager.atkPow_H);
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            Debug.Log(distination);
            rb.AddForce(distination * -3f, ForceMode.VelocityChange);

            Debug.Log("のけぞり大");
        }
        else if (other.CompareTag("E_Magic"))
        {
            Damage(damager.atkPow_L);
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            Debug.Log(distination);
            rb.AddForce(distination * -5f, ForceMode.VelocityChange);

            Debug.Log("ダウン");
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="AtkPow"></param>
    void Damage(int AtkPow)
    {
        //HP減らす
        hp -= AtkPow;

        //撃破処理
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            animator.SetTrigger("Dead");
        }

        // TODO UIに現在のHPを反映

    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        maxHp = GameData.instance.charaDataList[0].hp;
        hp = maxHp;
    }

    public GameObject hitEffectPrefab;
    public Vector3 effecOfset;

    /// <summary>
    /// HITエフェクト再生
    /// </summary>
    /// <param name="other"></param>
    public void GenerateEffect(GameObject other)
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.KatanaHit);
        GameObject effect = Instantiate(hitEffectPrefab, transform.position + effecOfset, transform.rotation);
        Destroy(effect, 2f);
    }



}
