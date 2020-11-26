using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBGM : MonoBehaviour
{
    public static ClearBGM instance;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Game7);
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
