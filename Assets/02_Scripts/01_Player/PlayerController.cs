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
    public TrailRenderer weapontrail;
    public Transform playerKatanaPos;

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
            inAction = true;
            anim.SetTrigger("L-Attack");

        }

        else if (Input.GetButtonDown("Fire1") && isKumiuchi == true)
        {
            inAction = true;
            anim.SetTrigger("Kumiuchi");
        }

        else if (Input.GetButtonDown("Fire1") && isOiuchi == true)
        {
            inAction = true;
            anim.SetTrigger("Oiuchi");
        }
    }

    /// <summary>
    /// 弱攻撃中身
    /// </summary>
    void LightAttackStart()
    {
        //アニメーションイベントに埋め込む
        transform.DOLocalMove(transform.forward * 0.1f, 0.2f).SetRelative();
        waponcollider.enabled = true;
    }

    /// <summary>
    /// 強攻撃アニメーション再生
    /// </summary>
    void HeavyAttack()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            inAction = true;
            anim.SetTrigger("H-Attack");
        }
    }

    /// <summary>
    /// 強攻撃中身
    /// </summary>
    void HeavyAttackStart()
    {
        //アニメーションイベントに埋め込む
        //rb.velocity = Vector3.zero;
        transform.DOLocalMove(transform.forward * 0.1f, 0.2f).SetRelative();
        //rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// 居合攻撃中身
    /// </summary>
    void IaiAttackStart()
    {
        //アニメーションイベントに埋め込む
        transform.DOLocalMove(transform.forward * 2.8f, 0.6f).SetRelative();
        p_Waponcollider.tag = "P_HeavyAttack";
        waponcollider.enabled = true;
    }

    /// <summary>
    /// パリィアニメーション再生
    /// </summary>
    void Parry()
    {
        //
        if (Input.GetButtonDown("Fire3"))
        {
            inAction = true;
            anim.SetTrigger("Parry");
        }

    }

    /// <summary>
    /// パリィアニメーション中身
    /// </summary>
    void ParryAction()
    {
        //アニメーションイベントに埋め込む
    }

    /// <summary>
    /// ダッシュ（回避）アニメーション再生
    /// </summary>
    void Dash()
    {
        //キー入力に応じてアニメーションを変更する
        if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.A) || Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.D))
        {
            inAction = true;
            anim.SetTrigger("F-Dash");
            transform.DOLocalMove(transform.forward * 1.5f, 0.8f).SetRelative();

        }
        else if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.W))
        {
            inAction = true;
            if (transform.localEulerAngles.y > 0f)//右向き
            {
                anim.SetTrigger("U-Dash");
                transform.DOLocalMove(transform.right * -1.3f, 0.8f).SetRelative();
                Debug.Log("→上回避");
            }
            else//左向き
            {
                anim.SetTrigger("D-Dash");
                transform.DOLocalMove(transform.right * 1.3f, 0.8f).SetRelative();
                Debug.Log("←上回避");
            }
        }
        else if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.S))
        {
            inAction = true;
            if (transform.localEulerAngles.y > 0f)
            {
                anim.SetTrigger("D-Dash");
                transform.DOLocalMove(transform.right * 1.3f, 0.8f).SetRelative();
                Debug.Log("→下回避");
            }
            if (transform.localEulerAngles.y < 0f)//右向き
            {
                anim.SetTrigger("U-Dash");
                transform.DOLocalMove(transform.right * -1.3f, 0.8f).SetRelative();
                Debug.Log("←下回避");
            }

        }
        else if (Input.GetButtonDown("Jump"))
        {
            inAction = true;
            anim.SetTrigger("B-Dash");
            transform.DOLocalMove(transform.forward * -1.5f, 0.8f).SetRelative();

        }
    }

    /// <summary>
    /// ダッシュアクション中身
    /// </summary>
    void DashAction()
    {
        //アニメーションイベントに埋め込む　無敵時間とモーションによる移動
    }

    /// <summary>
    /// フラグリセット（アイドルモーション開始時に埋め込む）
    /// </summary>
    public void Resetflag()
    {
        inAction = false;
        anim.ResetTrigger("RevAttack");
        p_Waponcollider.tag = "P_LightAttack";
        waponcollider.enabled = false;
        anim.ResetTrigger("L-Attack");
        anim.ResetTrigger("H-Attack");
        anim.ResetTrigger("Parry");
        anim.ResetTrigger("F-Dash");
        anim.ResetTrigger("U-Dash");
        anim.ResetTrigger("D-Dash");
        anim.ResetTrigger("B-Dash");
        anim.ResetTrigger("Kumiuchi");
        anim.ResetTrigger("Oiuchi");
        //コライダー全部オフ
        //トレイルオフ
    }

    /// <summary>
    /// 被ダメージモーションフラグリセット
    /// </summary>
    public void P_RevAttack()
    {
        anim.ResetTrigger("L-Attack");
        anim.ResetTrigger("H-Attack");
        anim.ResetTrigger("Parry");
        anim.ResetTrigger("F-Dash");
        anim.ResetTrigger("U-Dash");
        anim.ResetTrigger("D-Dash");
        anim.ResetTrigger("B-Dash");
        anim.ResetTrigger("Kumiuchi");
        anim.ResetTrigger("Oiuchi");
        //コライダー全部オフ
        //トレイルオフ
    }

    public void KatanaTransform()
    {
        Transform curKatanaPos = playerKatanaPos;

        curKatanaPos.transform.localPosition = new Vector3(-0.0439999998f, 0.200000003f, -0.0149999997f);
        curKatanaPos.transform.localRotation = Quaternion.Euler(351.700012f, 10.1860008f, 89.9479828f);
        //追い討ち時に武器の位置を変える
        //追い討ち時に武器の向きを変える
    }
    public void KatanaTransformReset()
    {
        Transform curKatanaPos = playerKatanaPos;

        curKatanaPos.transform.localPosition = new Vector3(-0.0439999998f, -0.289999992f, -0.0149999997f);
        curKatanaPos.transform.localRotation = Quaternion.Euler(0.198864356f, 190.186005f, 269.947632f);
        //ゲーム開始時の位置に戻す
        //ゲーム開始時の向きに戻す
    }


}
