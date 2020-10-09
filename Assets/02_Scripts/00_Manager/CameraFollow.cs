using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject follwPlayer;
    Vector3 playerPos;

    void Start()
    {
        follwPlayer = GameObject.Find("Player");
        playerPos = follwPlayer.transform.position;
    }

    void LateUpdate()
    {
        // 追従カメラ
        transform.position += follwPlayer.transform.position - playerPos;
        playerPos = follwPlayer.transform.position;

    }
}
