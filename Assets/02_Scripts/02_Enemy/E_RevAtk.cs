using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class E_RevAtk : MonoBehaviour
{
    bool isDead;
    public int maxHp;
    public int hp;
    public Slider hpSlider;

    public Collider oiuchiCollider;
    public Collider KumiuchiCollider;

    Animator animator;
    GameManager gameManager;
    EnemyController enemyController;
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
        other.gameObject.TryGetComponent(out P_Damager damager);

        // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
        distination = (other.transform.position - transform.position).normalized;
        distination = new Vector3(distination.x, 0f, 0f).normalized;

        //タグ判定
        if (other.CompareTag("P_LightAttack"))
        {
            Damage(damager.atkPow_L);
            animator.SetTrigger("Rev-Atk");
            GenerateEffect(other.gameObject);
            Debug.Log("のけぞり小");
        }
        else if (other.CompareTag("P_StunAttack"))
        {
            Damage(damager.atkPow_Kick);
            animator.SetTrigger("Rev-Stun");
            GenerateEffect(other.gameObject);
            transform.DOLocalMove(distination * -3f, 0.5f).SetRelative();
            Debug.Log("組み討ちやられ");
        }
        else if (other.CompareTag("P_HeavyAttack"))
        {
            Damage(damager.atkPow_H);
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            transform.DOLocalMove(distination * -3f, 0.5f).SetRelative();
            Debug.Log("ダウン");
        }
        else if (other.CompareTag("P_KickAtk"))
        {
            Damage(damager.atkPow_Kick);
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            transform.DOLocalMove(distination * -3f, 0.5f).SetRelative();
            Debug.Log("ダウン");
        }
        else if (other.CompareTag("P_IaiAtk"))
        {
            Damage(damager.atkPow_I);
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            transform.DOLocalMove(distination * -3f, 0.5f).SetRelative();
            Debug.Log("ダウン");
        }
        else if (other.CompareTag("Oiuchi"))
        {
            Damage(damager.atkPow_Oiuchi);
            animator.SetTrigger("Rev-Oiuchi");
            GenerateEffect(other.gameObject);
            Debug.Log("追い討ちHIT");
        }
        else if (other.CompareTag("Kumiuchi"))
        {
            Damage(damager.atkPow_Kumiuchi);
            animator.SetTrigger("Rev-Kumiuchi");
            GenerateEffect(other.gameObject);
            Debug.Log("組み討ちHIT");
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
            StartCoroutine(enemyController.DestroyEnemy(3.0f));
        }

        // TODO UIに現在のHPを反映
        UpdateHP(hp);
    }

    public void UpdateHP(int hp)
    {
        hpSlider.DOValue((float)hp / maxHp, 0.5f);
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        enemyController = GetComponent<EnemyController>();
        maxHp = GameData.instance.charaDataList[1].hp;
        hp = maxHp;
        UpdateHP(hp);
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


    public void OiuchiColON()
    {
        oiuchiCollider.enabled = true;
    }
    public void OiuchiColOFF()
    {
        oiuchiCollider.enabled = false;
    }

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
