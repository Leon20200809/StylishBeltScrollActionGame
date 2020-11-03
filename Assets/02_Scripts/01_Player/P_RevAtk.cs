using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class P_RevAtk : MonoBehaviour
{
    PlayerController playerController;
    Animator animator;
    GameManager gameManager;
    Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("E_Weapon"))
        {
            playerController.inAction = true;
            animator.SetTrigger("Rev-Atk");
            GenerateEffect(other.gameObject);
            Debug.Log("のけぞり小");
        }
        else if (other.CompareTag("E_ParryAtk"))
        {
            playerController.inAction = true;
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
            Vector3 distination = (other.transform.position - transform.position).normalized;
            distination = new Vector3(distination.x, 0f, 0f).normalized;
            Debug.Log(distination);
            rb.AddForce(distination * -3f, ForceMode.VelocityChange);

            Debug.Log("のけぞり大");
        }
        else if (other.CompareTag("P_HeavyAttack"))
        {
            playerController.inAction = true;
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
            Vector3 distination = (other.transform.position - transform.position).normalized;
            distination = new Vector3(distination.x, 0f, 0f).normalized;
            Debug.Log(distination);
            rb.AddForce(distination * -5f, ForceMode.VelocityChange);

            Debug.Log("ダウン");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
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



}
