using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUIManager : MonoBehaviour
{

    //情報取得用
    public Slider hpSlider;
    public P_RevAtk p_RevAtk;

    /// <summary>
    /// HP更新
    /// </summary>
    /// <param name="hp"></param>
    public void UpdateHP(int hp)
    {
        hpSlider.DOValue(hp, 0.5f);
    }

}
