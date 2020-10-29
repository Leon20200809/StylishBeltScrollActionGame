using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Attack : MonoBehaviour
{
    Collider wcollider;
    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("Player"))
        {
            
            wcollider.enabled = false;
            Debug.Log("ＨＩＴ");
            Debug.Log(wcollider);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        wcollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
