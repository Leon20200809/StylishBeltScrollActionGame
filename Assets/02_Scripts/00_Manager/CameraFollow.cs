using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followPlayer;
    Vector3 playerPos;

    void Start()
    {
        followPlayer = GameObject.Find("Player");
        playerPos = followPlayer.transform.position;
    }

    void LateUpdate()
    {
        // 追従カメラ
        transform.position += followPlayer.transform.position - playerPos;
        playerPos = followPlayer.transform.position;

    }
}
