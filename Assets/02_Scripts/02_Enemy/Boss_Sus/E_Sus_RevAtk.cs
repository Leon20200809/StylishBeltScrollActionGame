using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class E_Sus_RevAtk : E_RevAtk
{
    Animator animator;
    GameManager gameManager;
    [SerializeField]
    Vector3 distination;

    public GameObject hitEffectPrefab;
    public Vector3 effecOfset;

    void KumiuchiFinish()
    {
        transform.DOLocalMove(transform.forward * -1.5f, 0.3f).SetRelative();
    }

}
