using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class E_RevParry : MonoBehaviour
{
    Animator animator;
    GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("Parry"))
        {
            //animator.Play("DAMAGED00");
            animator.SetTrigger("Rev-Parry");
            GenerateEffect(other.gameObject);
            Debug.Log("パリィ成功");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject parryEffectPrefab;
    public Vector3 effecOfset;

    public void GenerateEffect(GameObject other)
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.KatanaHit);
        GameObject effect = Instantiate(parryEffectPrefab, transform.position + effecOfset, transform.rotation);
        Destroy(effect, 2f);
    }

}
