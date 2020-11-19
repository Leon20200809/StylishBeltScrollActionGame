using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    RaycastHit hit;

    public bool isEnable;
    public LayerMask layerMask;

    GameObject ObjectPrefab;

    void OnDrawGizmos()
    {
        if (!isEnable) return;

        var scale = transform.localScale.x * 0.5f;

        var isHit = Physics.BoxCast(transform.position, Vector3.one * scale, transform.forward, out hit, transform.rotation, layerMask);

        if (isHit)
        {
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, Vector3.one * scale * 2);
        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * 100);
        }
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {

            Debug.Log(hit.point);//デバッグログにヒットした場所を出す
                                 //レイを可視化
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
        }
    }

    private void Start()
    {

        for (int i = 0;  i < 5 ; i++)
        {
            GameObject leftLimitforword = Instantiate(ObjectPrefab, new Vector3(transform.position.x + i, 1, transform.position.z), Quaternion.Euler(0, 90, 0));

        }
    }
}
