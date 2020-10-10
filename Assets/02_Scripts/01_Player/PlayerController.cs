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

    //[Header("移動範囲")]
    //public MoveLimit moveLimit;

    private Rigidbody rb;
    private Animator anim;

    public TrailRenderer weapontrail;

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
        Move(x, z);
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

        Debug.Log(x);
        Debug.Log(z);

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
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("L-Attack");
        }
    }

    /// <summary>
    /// 弱攻撃中身
    /// </summary>
    void LightAttackStart()
    {
        //アニメーションイベントに埋め込む
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
    /// 強攻撃中身
    /// </summary>
    void HeavyAttackStart()
    {
        //アニメーションイベントに埋め込む
    }

    /// <summary>
    /// パリィアニメーション再生
    /// </summary>
    void Parry()
    {
        //
        if (Input.GetButtonDown("Fire3"))
        {
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
            anim.SetTrigger("F-Dash");
        }
        else if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.W))
        {
            anim.SetTrigger("U-Dash");
        }
        else if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.S))
        {
            anim.SetTrigger("D-Dash");
        }
        else if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("B-Dash");
        }


    }

    /// <summary>
    /// ダッシュアクション中身
    /// </summary>
    void DashAction()
    {
        //アニメーションイベントに埋め込む　無敵時間とモーションによる移動
    }


}
