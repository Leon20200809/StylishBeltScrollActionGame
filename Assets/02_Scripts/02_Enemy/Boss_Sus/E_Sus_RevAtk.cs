using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class E_Sus_RevAtk : MonoBehaviour
{
    Animator animator;
    GameManager gameManager;
    [SerializeField]
    Vector3 distination;


    private void OnTriggerEnter(Collider other)
    {
        // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
        distination = (other.transform.position - transform.position).normalized;
        distination = new Vector3(distination.x, 0f, 0f).normalized;

        //タグ判定
        if (other.CompareTag("P_LightAttack"))
        {
            //animator.Play("DAMAGED00");
            animator.SetTrigger("Rev-Atk");
            GenerateEffect(other.gameObject);
            Debug.Log("のけぞり小");
        }
        else if (other.CompareTag("P_StunAttack"))
        {
            animator.SetTrigger("Rev-Stun");
            GenerateEffect(other.gameObject);
            transform.DOLocalMove(distination * -1.5f, 0.3f).SetRelative();

            Debug.Log("のけぞり大");
        }
        else if (other.CompareTag("P_HeavyAttack"))
        {
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            transform.DOLocalMove(distination * -2.5f, 0.5f).SetRelative();

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
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

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

    void KumiuchiFinish()
    {
        transform.DOLocalMove(transform.forward * -1.5f, 0.3f).SetRelative();
    }


}
