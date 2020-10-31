using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class P_RevAtk : MonoBehaviour
{
    PlayerController playerController;
    Animator animator;
    GameManager gameManager;

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
            transform.DOLocalMove(transform.forward * -1.5f, 0.3f).SetRelative();

            Debug.Log("のけぞり大");
        }
        else if (other.CompareTag("P_HeavyAttack"))
        {
            playerController.inAction = true;
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            transform.DOLocalMove(transform.forward * -2.5f, 0.5f).SetRelative();

            Debug.Log("ダウン");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
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
