﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class E_SUSInput : MonoBehaviour
{
    [Header("プレイヤー操作")]
    public bool playerCont;

    [Header("移動速度")]
    public float moveSpeed;

    [Header("回転速度")]
    public float rotateSpeed;

    [Header("アクションフラグ")]
    public bool inAction;

    public GameObject e_Weaponcollider;
    public Collider e_Weaponcol;
    public TrailRenderer e_Weapontrail;

    private Rigidbody rb;
    private Animator anim;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    [SerializeField]
    GameManager gameManager;

    /// <summary>
    /// Player情報の初期設定
    /// </summary>
    private void InitPlayer()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (playerCont == true)
        {
            // キー入力
            float x = Input.GetAxisRaw(HORIZONTAL);
            float z = Input.GetAxisRaw(VERTICAL);

            // 移動
            if (inAction == false) Move(x, z);
        }
        else
        {
            return;
        }
        
    }

    /// <summary>
    /// 移動する
    /// </summary>
    /// <param name="x">X軸の移動値</param>
    /// <param name="z">Z軸の移動値</param>
    private void Move(float x, float z)
    {
        // キー入力値を正規化

        Vector3 moveDir = new Vector3(x, 0, z).normalized;

        // プレイヤーの移動
        rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);

        // 移動のアニメの同期
        anim.SetFloat("Speed", rb.velocity.magnitude);

        // 移動に合わせて向きを変える
        LookDirection(moveDir);

    }

    /// <summary>
    /// 向きを変える
    /// </summary>
    /// <param name="dir">移動値</param>
    private void LookDirection(Vector3 dir)
    {
        // ベクトル(向きと大きさ)の2乗の長さをfloatで戻す = 動いているかどうかの確認し、動いていなければ処理しない
        if (dir.sqrMagnitude <= 0f)
        {
            return;
        }

        // 横方向への入力がない場合には処理しない
        if (dir.x == 0)
        {
            return;
        }

        float pos = 0;
        if (dir.x > 0)
        {
            // 右
            pos = 90;
        }
        else
        {
            // 左
            pos = -90;
        }

        // プレイヤーの向きを進行方向に合わせる(上下移動の際には変更しない)
        transform.rotation = Quaternion.Euler(new Vector3(0, pos, 0));
    }


    // Start is called before the first frame update
    void Start()
    {
        InitPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCont == true)
        {
            //移動以外のアクション中は移動を制限すること
            L_Attack();
            H_Attack();
            M_Attack();
            S_Attack();
            Dash();

        }
        else
        {
            return;
        }

    }

    /// <summary>
    /// 弱攻撃アニメーション再生
    /// </summary>
    void L_Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            inAction = true;
            anim.SetTrigger("L-Atk");

        }
    }

    /// <summary>
    /// 弱攻撃中身
    /// </summary>
    void L_AttackStart()
    {
        //アニメーションイベントに埋め込む
        transform.DOLocalMove(transform.forward * 0.3f, 0.2f).SetRelative();
        SoundManager.instance.PlaySE(SoundManager.SE_Type.WhipAtk);
        e_Weaponcol.enabled = true;
        e_Weapontrail.enabled = true;
    }

    /// <summary>
    /// 強攻撃アニメーション再生
    /// </summary>
    void H_Attack()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            inAction = true;
            e_Weaponcollider.tag = "E_ParryAtk";
            anim.SetTrigger("H-Atk");
        }
    }

    /// <summary>
    /// 強攻撃中身
    /// </summary>
    void H_AttackStart()
    {
        //アニメーションイベントに埋め込む
        //rb.velocity = Vector3.zero;
        transform.DOLocalMove(transform.forward * 0.3f, 0.2f).SetRelative();
        SoundManager.instance.PlaySE(SoundManager.SE_Type.WhipAtk);
        e_Weaponcol.enabled = true;
        e_Weapontrail.enabled = true;
    }

    void S_Attack()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            inAction = true;
            anim.SetTrigger("M-Atk");
        }
    }

    void S_AttackReady()
    {
        GenerateExplosionReady();
    }
    void S_AttackStart()
    {
        GenerateExplosion();
    }


    void M_Attack()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            inAction = true;
            anim.SetTrigger("S-Atk");
            //e_Sus_MagicBall.MagicBallshot();
        }
    }

    public E_Sus_MagicBall e_Sus_MagicBall;
    void M_AttackStart()
    {
        e_Sus_MagicBall.MagicBallshot();
    }

    /// <summary>
    /// ボスダッシュ
    /// </summary>
    void Dash()
    {
        //前ダッシュ
        if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.A) || Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.D))
        {
            inAction = true;
            anim.SetTrigger("Dash");
            transform.DOLocalMove(transform.forward * 4f, 0.5f).SetRelative();

        }
    }


        /// <summary>
        /// パリィ成功アニメーション再生
        /// </summary>
        public void RevParryStart()
    {
        anim.SetTrigger("Rev-Parry");
    }

    public E_Sus_RevAtk e_Sus_RevAtk;
    /// <summary>
    /// フラグリセット（アイドルモーション開始時に埋め込む）
    /// </summary>
    public void Resetflag()
    {

        inAction = false;
        e_Weaponcol.enabled = false;
        e_Weapontrail.enabled = false;
        e_Sus_RevAtk.OiuchiColOFF();
        e_Sus_RevAtk.KumiuchiColOFF();
        anim.ResetTrigger("Rev-Atk");
        anim.ResetTrigger("Rev-Down");
        anim.ResetTrigger("L-Atk");
        anim.ResetTrigger("H-Atk");
        anim.ResetTrigger("M-Atk");
        anim.ResetTrigger("S-Atk");
        anim.ResetTrigger("Rev-Oiuchi");
        anim.ResetTrigger("Rev-Kumiuchi");
        anim.ResetTrigger("Rev-Stun");
        anim.ResetTrigger("Rev-Parry");

    }

    /// <summary>
    /// 被ダメージモーションフラグリセット
    /// </summary>
    public void P_RevAttack()
    {
        anim.ResetTrigger("Rev-Atk");
        anim.ResetTrigger("Rev-Down");
        anim.ResetTrigger("L-Atk");
        anim.ResetTrigger("H-Atk");
        anim.ResetTrigger("M-Atk");
        anim.ResetTrigger("S-Atk");
        anim.ResetTrigger("Rev-Oiuchi");
        anim.ResetTrigger("Rev-Kumiuchi");
        anim.ResetTrigger("Rev-Stun");
        e_Weaponcol.enabled = false;
        e_Weapontrail.enabled = false;
    }

    public Vector3 exeffecOfset;

    public void GenerateEffect(GameObject other)
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.E_HvAtk);
        GameObject hAtkEffect = Instantiate(EffectManager.instance.GetEffect(1), transform.position, transform.rotation);
        hAtkEffect.transform.parent = this.transform;
        Destroy(hAtkEffect, 1f);
    }

    public void GenerateExplosionReady()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.SusSAtkReady);
        GameObject sAtkEffect = Instantiate(EffectManager.instance.GetEffect(4), transform.position + exeffecOfset, Quaternion.Euler(-90, 0, 0));
        sAtkEffect.transform.parent = this.transform;
        Destroy(sAtkEffect, 3f);
    }

    public void GenerateExplosion()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.SusSAtk);
        GameObject sAtkEffect = Instantiate(EffectManager.instance.GetEffect(3), transform.position, transform.rotation);
        Destroy(sAtkEffect, 4f);
    }



}
