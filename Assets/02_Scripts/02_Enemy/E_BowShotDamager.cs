using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_BowShotDamager : E_DamagerBase

{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            wcollider.enabled = false;
            Debug.Log("弓攻撃HIT");
            Destroy(this.gameObject);
        }

    }
    // Start is called before the first frame update

}
