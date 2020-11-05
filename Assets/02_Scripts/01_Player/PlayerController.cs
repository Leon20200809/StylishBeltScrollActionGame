using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed;

    [Header("回転速度")]
    public float rotateSpeed;

    [Header("アクションフラグ")]
    public bool inAction;
    public bool isKumiuchi;
    public bool isOiuchi;

    public GameObject p_Waponcollider;
    public Collider waponcollider;
    public Collider kickcollider;
    public Collider parrycollider;
    public TrailRenderer weapontrail;
    public TrailRenderer kicktrail;
    public TrailRenderer parrytrail;
    public Transform playerKatanaPos;
    public P_KumiuchiCam kumiuchiCam;

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
        // プレイヤーの移動範囲を確認し、範囲内になるように制限
        //float posX = Mathf.Clamp(transform.position.x, moveLimit.horizontalLimit.left, moveLimit.horizontalLimit.right);
        //float posZ = Mathf.Clamp(transform.position.z, moveLimit.depthLimit.back, moveLimit.depthLimit.forword);

        //エリアの情報に合わせて、プレイヤーの移動範囲を確認し、範囲内になるように制限
        float posX = Mathf.Clamp(transform.position.x, gameManager.leftLimitPos, gameManager.rightLimitPos);
        float posZ = Mathf.Clamp(transform.position.z, gameManager.backLimitPos, gameManager.forwordLimitPos);

        // 現在位置を更新
        transform.position = new Vector3(posX, transform.position.y, posZ);

        // キー入力
        float x = Input.GetAxisRaw(HORIZONTAL);
        float z = Input.GetAxisRaw(VERTICAL);

        // 移動
        if (inAction == false) Move(x, z);
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
        //移動以外のアクション中は移動を制限すること
        LightAttack();
        HeavyAttack();
        Dash();
        Parry();


    }

    /// <summary>
    /// 弱攻撃アニメーション再生
    /// </summary>
    void LightAttack()
    {
        if (Input.GetButtonDown("Fire1") && isKumiuchi == false && isOiuchi == false)
        {
            anim.SetTrigger("L-Attack");
        }

        else if (Input.GetButtonDown("Fire1") && isKumiuchi == true)
        {
            anim.SetTrigger("Kumiuchi");
            kumiuchiCam.KumiuchiStaging();
        }

        else if (Input.GetButtonDown("Fire1") && isOiuchi == true)
        {
            anim.SetTrigger("Oiuchi");
        }
    }


    /// <summary>
    /// 強攻撃アニメーション再生
    /// </summary>
    void HeavyAttack()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("H-Attack");
        }
    }

    /// <summary>
    /// パリィアニメーション再生
    /// </summary>
    void Parry()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            anim.SetTrigger("Parry");
        }

    }

    /// <summary>
    /// ダッシュ（回避）アニメーション再生
    /// </summary>
    void Dash()
    {
        //前ダッシュ
        if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.A) || Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.D))
        {
            anim.SetTrigger("F-Dash");

        }

        //上回避
        else if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.W))
        {
            Debug.Log(transform.localEulerAngles.y);

            if (transform.localEulerAngles.y < 99f)//右向き
            {
                anim.SetTrigger("U-Dash");
            }
            if (transform.localEulerAngles.y > 269f)//左向き
            {
                anim.SetTrigger("D-Dash");
            }
        }

        //下回避
        else if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.S))
        {
            if (transform.localEulerAngles.y < 99f)
            {
                anim.SetTrigger("D-Dash");
            }
            if (transform.localEulerAngles.y > 269f)//右向き
            {
                anim.SetTrigger("U-Dash");
            }

        }

        //バックステップ
        else if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("B-Dash");

        }
    }

    //================= アニメーションイベント用メソッド ===================//　ここから

    /// <summary>
    /// 弱攻撃中身
    /// </summary>
    void LightAttackStart()
    {
        transform.DOLocalMove(transform.forward * 0.1f, 0.2f).SetRelative();
        waponcollider.enabled = true;
        weapontrail.enabled = true;
    }

    /// <summary>
    /// 強攻撃アニメーションイベント用
    /// </summary>
    void HeavyAttackStart()
    {
        p_Waponcollider.tag = "P_HeavyAttack";
        transform.DOLocalMove(transform.forward * 0.1f, 0.2f).SetRelative();
        waponcollider.enabled = true;
        weapontrail.enabled = true;
    }

    /// <summary>
    /// パリィアニメーションイベント用
    /// </summary>
    void ParryAction()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Parry);
        transform.DOLocalMove(transform.forward * 0.2f, 0.2f).SetRelative();
        parrycollider.enabled = true;
        parrytrail.enabled = true;
    }

    /// <summary>
    /// 居合攻撃アニメーションイベント用
    /// </summary>
    void IaiAttackStart()
    {
        //アニメーションイベントに埋め込む
        transform.DOLocalMove(transform.forward * 4.0f, 0.6f).SetRelative();
        p_Waponcollider.tag = "P_HeavyAttack";
        waponcollider.enabled = true;
        weapontrail.enabled = true;
    }

    /// <summary>
    /// 残月攻撃アニメーションイベント用
    /// </summary>
    void ZangetsuAttackStart()
    {
        transform.DOLocalMove(transform.forward * 2.0f, 0.3f).SetRelative();
        p_Waponcollider.tag = "P_HeavyAttack";
        waponcollider.enabled = true;
        weapontrail.enabled = true;
    }

    /// <summary>
    /// 蹴り攻撃アニメーションイベント用
    /// </summary>
    void KickAttackStart()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Kick);
        transform.DOLocalMove(transform.forward * 0.5f, 0.2f).SetRelative();
        kickcollider.enabled = true;
        kicktrail.enabled = true;
    }

    /// <summary>
    /// スタン攻撃アニメーションイベント用
    /// </summary>
    void StunAttackStart()
    {
        p_Waponcollider.tag = "P_StunAttack";
        transform.DOLocalMove(transform.forward * 0.1f, 0.2f).SetRelative();
        waponcollider.enabled = true;
        weapontrail.enabled = true;
    }


    /// <summary>
    /// 前ダッシュアニメーションイベント用
    /// </summary>
    void DashAction_F()
    {
        //アニメーションイベントに埋め込む　無敵時間とモーションによる移動
        transform.DOLocalMove(transform.forward * 2.0f, 0.5f).SetRelative();

    }
    void DashAction_U()
    {
        //アニメーションイベントに埋め込む　無敵時間とモーションによる移動
        transform.DOLocalMove(transform.right * -1.3f, 0.5f).SetRelative();

    }

    void DashAction_D()
    {
        //アニメーションイベントに埋め込む　無敵時間とモーションによる移動
        transform.DOLocalMove(transform.right * 1.3f, 0.5f).SetRelative();

    }
    void DashAction_B()
    {
        //アニメーションイベントに埋め込む　無敵時間とモーションによる移動
        transform.DOLocalMove(transform.forward * -2.0f, 0.5f).SetRelative();
    }


    /// <summary>
    /// 追い討ち時の刀位置補正アニメーションイベント用
    /// </summary>
    public void KatanaTransform()
    {
        Transform curKatanaPos = playerKatanaPos;

        curKatanaPos.transform.localPosition = new Vector3(-0.0439999998f, 0.200000003f, -0.0149999997f);
        curKatanaPos.transform.localRotation = Quaternion.Euler(351.700012f, 10.1860008f, 89.9479828f);
    }

    /// <summary>
    /// 追い討ち時の刀位置リセットアニメーションイベント用
    /// </summary>
    public void KatanaTransformReset()
    {
        Transform curKatanaPos = playerKatanaPos;

        curKatanaPos.transform.localPosition = new Vector3(-0.0439999998f, -0.289999992f, -0.0149999997f);
        curKatanaPos.transform.localRotation = Quaternion.Euler(0.198864356f, 190.186005f, 269.947632f);
    }


    void KumiuchiFinish()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Kick);
    }





    //=========== Smb用メソッド ===================//　ここから

    /// <summary>
    /// フラグリセット
    /// </summary>
    public void Resetflag()
    {
        inAction = false;
        anim.ResetTrigger("Rev-Atk");
        anim.ResetTrigger("Rev-Down");
        p_Waponcollider.tag = "P_LightAttack";
        waponcollider.enabled = false;
        weapontrail.enabled = false;
        isOiuchi = false;
        isKumiuchi = false;
        kickcollider.enabled = false;
        kicktrail.enabled = false;
        parrycollider.enabled = false;
        parrytrail.enabled = false;
        anim.ResetTrigger("L-Attack");
        anim.ResetTrigger("H-Attack");
        anim.ResetTrigger("Parry");
        anim.ResetTrigger("F-Dash");
        anim.ResetTrigger("U-Dash");
        anim.ResetTrigger("D-Dash");
        anim.ResetTrigger("B-Dash");
        anim.ResetTrigger("Kumiuchi");
        anim.ResetTrigger("Oiuchi");
    }

    /// <summary>
    /// 被ダメージモーションフラグリセット
    /// </summary>
    public void P_RevAttack()
    {
        inAction = true;
        anim.ResetTrigger("L-Attack");
        anim.ResetTrigger("H-Attack");
        anim.ResetTrigger("Parry");
        anim.ResetTrigger("F-Dash");
        anim.ResetTrigger("U-Dash");
        anim.ResetTrigger("D-Dash");
        anim.ResetTrigger("B-Dash");
        anim.ResetTrigger("Kumiuchi");
        anim.ResetTrigger("Oiuchi");
        p_Waponcollider.tag = "P_LightAttack";
        waponcollider.enabled = false;
        weapontrail.enabled = false;
        isOiuchi = false;
        isKumiuchi = false;
        kickcollider.enabled = false;
        kicktrail.enabled = false;
        parrycollider.enabled = false;
        parrytrail.enabled = false;
    }
    /// <summary>
    /// ハイパーアーマー
    /// </summary>
    public void P_HypAmr()
    {
        anim.ResetTrigger("Rev-Atk");
        anim.ResetTrigger("Rev-Down");
        inAction = true;
    }


    //=========== OnTrigger用メソッド ===================//


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("OiuchiCol"))
        {
            StartCoroutine(OiuchiFlag(2.0f));
        }
        else if (other.CompareTag("KumiuchiCol"))
        {
            StartCoroutine(KumiuchiFlag(2.0f));
        }
        return;
    }

    IEnumerator OiuchiFlag(float waitTime = 0.0f)
    {
        isOiuchi = true;
        p_Waponcollider.tag = "Oiuchi";
        yield return new WaitForSeconds(waitTime);
        isOiuchi = false;
        p_Waponcollider.tag = "P_LightAttack";
    }
    IEnumerator KumiuchiFlag(float waitTime = 0.0f)
    {
        isKumiuchi = true;
        p_Waponcollider.tag = "Kumiuchi";
        yield return new WaitForSeconds(waitTime);
        isKumiuchi = false;
        p_Waponcollider.tag = "P_LightAttack";
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OiuchiCol"))
        {
            isOiuchi = false;
            p_Waponcollider.tag = "P_LightAttack";
            Debug.Log("追い討ち範囲外");
        }

        else if (other.CompareTag("KumiuchiCol"))
        {
            isKumiuchi = false;
            p_Waponcollider.tag = "P_LightAttack";
        }
    }


}
