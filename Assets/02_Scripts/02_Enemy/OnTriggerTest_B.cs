using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerTest_B : MonoBehaviour
{
    Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("P_LightAttack"))
        {
            //animator.Play("DAMAGED00");
            animator.SetTrigger("E_RevAttack");
            Debug.Log("のけぞり小");
        }
        else if (other.CompareTag("P_MidAttack"))
        {
            Debug.Log("のけぞり大");
        }
        else if (other.CompareTag("P_HeavyAttack"))
        {
            Debug.Log("ダウン");
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

}
