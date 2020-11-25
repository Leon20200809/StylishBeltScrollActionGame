using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class E_SUSInput : E_Input
{
    public E_Sus_RevAtk e_Sus_RevAtk;

    //飛び道具プレファブ
    public E_Sus_MagicBall e_Sus_MagicBall;

    // Update is called once per frame
    protected override void Update()
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
            anim.SetTrigger("L-Atk");
        }
    }

    /// <summary>
    /// 強攻撃アニメーション再生
    /// </summary>
    void H_Attack()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("H-Atk");
        }
    }

    /// <summary>
    /// 飛び道具アニメーション再生
    /// </summary>
    void M_Attack()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            anim.SetTrigger("M-Atk");
        }
    }

    /// <summary>
    /// 必殺技アニメーション再生
    /// </summary>
    void S_Attack()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetTrigger("S-Atk");
        }
    }

    /// <summary>
    /// ボスダッシュアニメーション再生
    /// </summary>
    void Dash()
    {
        //前ダッシュ
        if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.A) || Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.D))
        {
            anim.SetTrigger("Dash");
        }
    }


    //================= アニメーションイベント用メソッド ===================//　ここから

    /// <summary>
    /// 弱攻撃アニメーションイベント用
    /// </summary>
    void L_AttackStart()
    {
        transform.DOLocalMove(transform.forward * 0.3f, 0.2f).SetRelative();
        SoundManager.instance.PlaySE(SoundManager.SE_Type.WhipAtk);
        e_Weaponcol.enabled = true;
        e_Weapontrail.enabled = true;
    }

    /// <summary>
    /// 強攻撃アニメーションイベント用
    /// </summary>
    void H_AttackStart()
    {
        e_Weaponcollider.tag = "E_ParryAtk";
        transform.DOLocalMove(transform.forward * 0.3f, 0.2f).SetRelative();
        SoundManager.instance.PlaySE(SoundManager.SE_Type.WhipAtk);
        e_Weaponcol.enabled = true;
        e_Weapontrail.enabled = true;
    }

    /// <summary>
    /// ダッシュアニメーションイベント用
    /// </summary>
    void DashAction()
    {
        transform.DOLocalMove(transform.forward * 4f, 0.5f).SetRelative();
    }


    /// <summary>
    /// 必殺技溜めアニメーションイベント用
    /// </summary>
    void S_AttackReady()
    {
        GenerateExplosionReady();
    }
    /// <summary>
    /// 必殺技アニメーションイベント用
    /// </summary>
    void S_AttackStart()
    {
        GenerateExplosion();
    }


    /// <summary>
    /// 飛び道具アニメーションイベント用
    /// </summary>
    void M_AttackStart()
    {
        e_Sus_MagicBall.MagicBallshot();
    }

    /// <summary>
    /// パリィ成功アニメーション再生
    /// </summary>
    public new void RevParryStart()
    {
        anim.SetTrigger("Rev-Parry");
    }

    public Vector3 exeffecOfset;

    /// <summary>
    /// 必殺技溜め用エフェクト再生
    /// </summary>
    public void GenerateExplosionReady()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.SusSAtkReady);
        GameObject sAtkEffect = Instantiate(EffectManager.instance.GetEffect(4), transform.position + exeffecOfset, Quaternion.Euler(-90, 0, 0));
        sAtkEffect.transform.parent = this.transform;
        Destroy(sAtkEffect, 3f);
    }


    /// <summary>
    /// 必殺技用エフェクト再生
    /// </summary>
    public void GenerateExplosion()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.SusSAtk);
        GameObject sAtkEffect = Instantiate(EffectManager.instance.GetEffect(3), transform.position, transform.rotation);
        Destroy(sAtkEffect, 4f);
    }


    //=============== Smb用メソッド ===================//　ここから
    /// <summary>
    /// フラグリセット（アイドルモーション開始時に埋め込む）
    /// </summary>
    public void Resetflag()
    {

        inAction = false;
        e_Weaponcol.enabled = false;
        e_Weapontrail.enabled = false;
        e_Parrycollider.enabled = false;
        e_RevAtk.OiuchiColOFF();
        e_RevAtk.KumiuchiColOFF();
        anim.ResetTrigger("Rev-Atk");
        anim.ResetTrigger("Rev-Down");
        anim.ResetTrigger("L-Atk");
        anim.ResetTrigger("H-Atk");
        anim.ResetTrigger("Rev-Oiuchi");
        anim.ResetTrigger("Rev-Kumiuchi");
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
        anim.ResetTrigger("Rev-Oiuchi");
        anim.ResetTrigger("Rev-Kumiuchi");
        e_Weaponcol.enabled = false;
        e_Weapontrail.enabled = false;
    }
}
