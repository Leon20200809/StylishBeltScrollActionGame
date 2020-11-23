using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Sus_MagicBall : MonoBehaviour

{
    public GameObject sus_MagicBallPrefab;
    public Vector3 effecOfset;
    public float ballSpeed;

    /// <summary>
    /// マジックボール射出
    /// </summary>
    public void MagicBallshot()
    {
        GameObject sus_MagicBall = Instantiate(sus_MagicBallPrefab, transform.position + effecOfset, transform.rotation);
        Rigidbody mbRb = sus_MagicBall.GetComponent<Rigidbody>();
        mbRb.AddForce(transform.forward * ballSpeed);
        SoundManager.instance.PlaySE(SoundManager.SE_Type.MagicBall);
        Destroy(sus_MagicBall, 4f);

    }
}
