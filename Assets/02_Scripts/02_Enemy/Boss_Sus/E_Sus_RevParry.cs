using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class E_Sus_RevParry : MonoBehaviour
{
    public E_SUSInput e_Input;
    GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("Parry"))
        {
            e_Input.RevParryStart();
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
        SoundManager.instance.PlaySE(SoundManager.SE_Type.ParryS);
        GameObject effect = Instantiate(parryEffectPrefab, transform.position + effecOfset, transform.rotation);
        Destroy(effect, 2f);
    }

}
