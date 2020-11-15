using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_BowShot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Vector3 effecOfset;
    public float arrowSpeed;


    /// <summary>
    /// 矢射出
    /// </summary>
    public void Bowshot()
    {
        GameObject arrowShot = Instantiate(arrowPrefab, transform.position + effecOfset, Quaternion.Euler(0, 0, 90));
        Rigidbody mbRb = arrowShot.GetComponent<Rigidbody>();
        mbRb.AddForce(transform.forward * arrowSpeed);
        SoundManager.instance.PlaySE(SoundManager.SE_Type.BowAtk);
        Destroy(arrowShot, 4f);
    }
}
