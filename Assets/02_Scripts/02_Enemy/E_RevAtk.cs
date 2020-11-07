using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class E_RevAtk : MonoBehaviour
{
    Animator animator;
    GameManager gameManager;
    EnemyController enemyController;
    Rigidbody rb;
    [SerializeField]
    Vector3 distination;

    private void OnTriggerEnter(Collider other)
    {
        // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
        distination = (other.transform.position - transform.position).normalized;
        distination = new Vector3(distination.x, 0f, 0f).normalized;
        Debug.Log(distination);

        //Damage();

        //タグ判定
        if (other.CompareTag("P_LightAttack"))
        {
            animator.SetTrigger("Rev-Atk");
            GenerateEffect(other.gameObject);
            Debug.Log("のけぞり小");
        }
        else if (other.CompareTag("P_StunAttack"))
        {
            animator.SetTrigger("Rev-Stun");
            GenerateEffect(other.gameObject);
            rb.AddForce(distination * -3f, ForceMode.VelocityChange);

            Debug.Log("組み討ちやられ");
        }
        else if (other.CompareTag("P_HeavyAttack"))
        {
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            rb.AddForce(distination * -5f, ForceMode.VelocityChange);

            Debug.Log("ダウン");
        }
        else if (other.CompareTag("Oiuchi"))
        {
            animator.SetTrigger("Rev-Oiuchi");
            GenerateEffect(other.gameObject);

            Debug.Log("追い討ちHIT");
        }
        else if (other.CompareTag("Kumiuchi"))
        {
            animator.SetTrigger("Rev-Kumiuchi");
            GenerateEffect(other.gameObject);

            Debug.Log("組み討ちHIT");

        }
    }

    public void Damage()
    {
        //HP減らす

        //撃破処理
        animator.SetTrigger("Dead");
        StartCoroutine(enemyController.DestroyEnemy(3.0f));
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        enemyController = GetComponent<EnemyController>();
    }

    public Vector3 hitEffecOfset;

    /// <summary>
    /// HITエフェクト再生
    /// </summary>
    /// <param name="other"></param>
    public void GenerateEffect(GameObject other)
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.KatanaHit);
        GameObject hitEffect = Instantiate(EffectManager.instance.GetEffect(0), transform.position + hitEffecOfset, transform.rotation);
        Destroy(hitEffect, 2f);
    }

    public Collider oiuchiCollider;

    public void OiuchiColON()
    {
        oiuchiCollider.enabled = true;
    }
    public void OiuchiColOFF()
    {
        oiuchiCollider.enabled = false;
    }
    public Collider KumiuchiCollider;

    public void KumiuchiColON()
    {
        KumiuchiCollider.enabled = true;
    }
    public void KumiuchiColOFF()
    {
        KumiuchiCollider.enabled = false;
    }

    /// <summary>
    /// 組み討ち食らいアニメーションイベント用
    /// </summary>
    /// <param name="other"></param>
    void KumiuchiFinish(Collider other)
    {
        // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
        rb.AddForce(distination * -5f, ForceMode.VelocityChange);
    }


}
