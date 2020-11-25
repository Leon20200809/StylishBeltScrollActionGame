using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class E_Sus_RevAtk : E_RevAtk
{
    public GameObject hitEffectPrefab;
    public Vector3 effecOfset;

    void KumiuchiFinish()
    {
        transform.DOLocalMove(transform.forward * -1.5f, 0.3f).SetRelative();
    }

}
