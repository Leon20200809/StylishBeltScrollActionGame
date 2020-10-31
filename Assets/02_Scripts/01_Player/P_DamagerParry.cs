using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_DamagerParry : MonoBehaviour
{
    Collider wcollider;
    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("E_ParryAtk"))
        {
            wcollider.enabled = false;
            Debug.Log("プレイヤーパリィ");
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
