using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_BowShotDamager : E_DamagerBase

{
    // Start is called before the first frame update

    protected override void Attack_Successful()
    {
        Debug.Log("弓攻撃HIT");
        Destroy(this.gameObject);
    }

}
