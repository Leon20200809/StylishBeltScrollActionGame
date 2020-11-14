using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_KumiuchiCam : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject kumiuchiCam;

    public void KumiuchiStaging()
    {
        StartCoroutine(CamChange(3.0f));
    }

    IEnumerator CamChange(float waitTime = 0.0f)
    {
        mainCam.SetActive(false);
        kumiuchiCam.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        mainCam.SetActive(true);
        kumiuchiCam.SetActive(false);

    }


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
