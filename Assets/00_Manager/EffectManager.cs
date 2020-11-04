using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// エフェクト管理クラス
/// </summary>
public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    //エフェクト管理
    public enum Effect_Type
    {
        //エフェクト列挙子を登録
        Hit,
        Aura,
        MagicBall,
        ParrySuccse,
        MagicExprotion,
        MagicExprotionReady,
        
        

    }

    public GameObject[] effect_Prefabs;

    public GameObject GetEffect(int effectNo) //列挙子で指定したい(Effect_Type effect_Type)
    {
        return effect_Prefabs[effectNo];
    }

    void Awake()
    {
        //シングルトンかつ、シーン遷移しても破棄されないようにする
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



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
