﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

/// <summary>
/// 音源管理クラス
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    //BGM管理
    public enum BGM_Type
    {
        //BGM列挙子を登録
        Game1,
        Game2,

    }

    //SE管理
    public enum SE_Type
    {
        //SE列挙子を登録
        KatanaFuri,
        KatanaHit,
        KatanaIai,
        KatanaTuki,
        Kick,
        Parry,
        ParryS,
        E_HvAtk,
        MagicBall,
        WhipAtk,
        SusSAtk,
        SusSAtkReady,
        BowAtk,
        Dash,

    }

    //VOICE管理
    public enum VOICE_Type
    {
        //VOICE列挙子を登録
        U1,
        U2,

    }

    //クロスフェード時間
    public const float CROSS_FADE_TIME = 1.0f;

    //ボリューム関連
    public float BGM_Volume = 0.1f;
    public float SE_Volume = 0.2f;
    public float VOICE_Volume = 0.2f;
    public bool Mute = false;

    //=== AudioClip ===→楽器
    public AudioClip[] BGM_Clips;
    public AudioClip[] SE_Clips;
    public AudioClip[] VOICE_Clips;

    //SE用オーディオミキサー→指揮者
    public AudioMixer audioMixer;

    //=== AudioSource ===→演奏者
    private AudioSource[] BGM_Sources = new AudioSource[2];
    private AudioSource[] SE_Sources = new AudioSource[16];
    private AudioSource[] VOICE_Sources = new AudioSource[20];

    private bool isCrossFading;
    private int currentBgmIndex = 999;


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

        //BGM用 AudioSource追加
        BGM_Sources[0] = gameObject.AddComponent<AudioSource>();
        BGM_Sources[1] = gameObject.AddComponent<AudioSource>();

        //SE用 AudioSource追加
        for (int i = 0; i < SE_Sources.Length; i++)
        {
            SE_Sources[i] = gameObject.AddComponent<AudioSource>();
        }

        //VOICE用 AudioSource追加
        for (int i = 0; i < VOICE_Sources.Length; i++)
        {
            VOICE_Sources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ボリューム設定
        if (!isCrossFading)
        {
            BGM_Sources[0].volume = BGM_Volume;
            BGM_Sources[1].volume = BGM_Volume;
        }

        foreach (AudioSource source in SE_Sources)
        {
            source.volume = SE_Volume;
        }

        foreach (AudioSource source in VOICE_Sources)
        {
            source.volume = VOICE_Volume;
        }
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="bgmType"></param>
    /// <param name="loopFlg"></param>
    public void PlayBGM(BGM_Type bgmType, bool loopFlg = true)
    {
        int index = (int)bgmType;
        currentBgmIndex = index;

        if (index < 0 || BGM_Clips.Length <= index)
        {
            return;
        }

        //同じBGMの場合は何もしない
        if (BGM_Sources[0].clip != null && BGM_Sources[0].clip == BGM_Clips[index])
        {
            return;
        }
        else if (BGM_Sources[1].clip != null && BGM_Sources[1].clip == BGM_Clips[index])
        {
            return;
        }

        //フェードでBGM開始
        if (BGM_Sources[0].clip == null && BGM_Sources[1].clip == null)
        {
            BGM_Sources[0].loop = loopFlg;
            BGM_Sources[0].clip = BGM_Clips[index];
            BGM_Sources[0].Play();
        }
        else
        {
            //クロスフェード処理
            StartCoroutine(CrossFadeChangeBMG(index, loopFlg));
        }
    }

    /// <summary>
    /// BGMのクロスフェード処理
    /// </summary>
    /// <param name="index"></param>
    /// <param name="loopFlg"></param>
    /// <returns></returns>
    private IEnumerator CrossFadeChangeBMG(int index, bool loopFlg)
    {
        isCrossFading = true;
        if (BGM_Sources[0].clip != null)
        {
            // [0]が再生されている場合、[0]の音量を徐々に下げて、[1]を新しい曲として再生
            BGM_Sources[1].volume = 0;
            BGM_Sources[1].clip = BGM_Clips[index];
            BGM_Sources[1].loop = loopFlg;
            BGM_Sources[1].Play();
            BGM_Sources[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            BGM_Sources[0].Stop();
            BGM_Sources[0].clip = null;
        }
        else
        {
            // [1]が再生されている場合、[1]の音量を徐々に下げて、[0]を新しい曲として再生
            BGM_Sources[0].volume = 0;
            BGM_Sources[0].clip = BGM_Clips[index];
            BGM_Sources[0].loop = loopFlg;
            BGM_Sources[0].Play();
            BGM_Sources[1].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            BGM_Sources[1].Stop();
            BGM_Sources[1].clip = null;
        }
        isCrossFading = false;
    }

    /// <summary>
    /// BGM完全停止
    /// </summary>
    public void StopBGM()
    {
        BGM_Sources[0].Stop();
        BGM_Sources[1].Stop();
        BGM_Sources[0].clip = null;
        BGM_Sources[1].clip = null;
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="sE_Type"></param>
    public void PlaySE(SE_Type sE_Type)
    {
        int index = (int)sE_Type;
        if (index < 0 || SE_Clips.Length <= index)
        {
            return;
        }

        //再生中ではないAudioSouceをつかってSEを鳴らす
        foreach (AudioSource source in SE_Sources)
        {
            if (false == source.isPlaying)
            {
                source.clip = SE_Clips[index];
                source.Play();
                return;
            }
        }
    }

    /// <summary>
    /// VOICE再生
    /// </summary>
    /// <param name="vO_Type"></param>
    public void PlayVOICE(VOICE_Type vO_Type)
    {
        int index = (int)vO_Type;
        if (index < 0 || VOICE_Clips.Length <= index)
        {
            return;
        }

        //再生中ではないAudioSouceをつかってSEを鳴らす
        foreach (AudioSource source in VOICE_Sources)
        {
            if (false == source.isPlaying)
            {
                source.clip = VOICE_Clips[index];
                source.Play();
                return;
            }
        }
    }

    /// <summary>
    /// SE停止
    /// </summary>
    public void StopSE()
    {
        //全てのSE用のAudioSouceを停止する
        foreach (AudioSource source in SE_Sources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    /// <summary>
    /// BGM一時停止
    /// </summary>
    public void MuteBGM()
    {
        BGM_Sources[0].Stop();
        BGM_Sources[1].Stop();
    }

    /// <summary>
    /// 一時停止した同じBGMを再生(再開)
    /// </summary>
    public void ResumeBGM()
    {
        BGM_Sources[0].Play();
        BGM_Sources[1].Play();
    }

    ////* 未使用 *////

    /// <summary>
    /// AudioMixer設定
    /// </summary>
    /// <param name="vol"></param>
    public void SetAudioMixerVolume(float vol)
    {
        if (vol == 0)
        {
            audioMixer.SetFloat("volumeSE", -80);
        }
        else
        {
            audioMixer.SetFloat("volumeSE", 0);
        }
    }
}
