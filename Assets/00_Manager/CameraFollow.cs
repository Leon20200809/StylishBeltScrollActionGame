using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followPlayer;
    Vector3 playerPos;

    public void Setup_Camera(GameObject player)
    {
        followPlayer = player;// GameObject.Find("Player");
        playerPos = followPlayer.transform.position;
    }

    void LateUpdate()
    {
        if (followPlayer == null)
        {
            return;
        }
        // 追従カメラ
        transform.position += followPlayer.transform.position - playerPos;
        playerPos = followPlayer.transform.position;

    }
}
