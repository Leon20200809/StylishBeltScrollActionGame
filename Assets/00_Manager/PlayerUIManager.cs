using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUIManager : MonoBehaviour
{

    //情報取得用
    public Slider hpSlider;
    public PlayerController playerController;

    //HP更新
    public void UpdateHP(int hp)
    {
        hpSlider.DOValue(hp, 0.5f);
    }

    /// <summary>
    /// プレイヤーステータスの更新
    /// </summary>
    /// <param name="playerManager">プレイヤーマネージャー</param>
    public void Init(P_RevAtk playerHp)
    {
        //HP,SP
        hpSlider.maxValue = playerHp.maxHp;
        hpSlider.value = playerHp.hp;

    }

}
