using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ComboDetail : MonoBehaviour
{

    [SerializeField]// 吹き出しの大きさの制御用(子オブジェクトである文字の大きさも一緒に制御される)　ComboDetail ゲームオブジェクトの Image コンポーネントをアサインする
    private Image imgCombo;

    [SerializeField]// コンボ数の表示制御用。ComboDetail ゲームオブジェクトの 子オブジェクトの txtHitCount ゲームオブジェクトの Text コンポーネントをアサインする
    private Text txtHitCount;            

    [SerializeField]// コンボ数表示を徐々に見えなくする演出に利用する。ComboDetail ゲームオブジェクトの CanvasGroup コンポーネントをアサインする
    private CanvasGroup canvasGroup;

    /// <summary>
    /// ComboDetailの設定。生成時に呼び出されることで Startメソッドのように機能させる
    /// </summary>
    /// <param name="comboCount"></param>
    public void SetUpComboDetail(int comboCount)
    {
        // ヒット数を表示
        txtHitCount.text = comboCount + "Hit!";

        // ヒット数に応じてサイズを調整するための変数を用意
        Vector3 scale = Vector3.one * (1 + comboCount * 0.1f);

        // 毎回同じ位置に表示されないように乱数を利用して調整(重複して生成される場合にも同じ位置に重ならないようにする演出にもなる)
        transform.position = new Vector2(transform.position.x + Random.Range(-7.5f, 7.5f), transform.position.y + Random.Range(-7.5f, 7.5f));

        // Sequenceを初期化して利用できるようにする
        Sequence sequence = DOTween.Sequence();

        // 一瞬大きくなるアニメ演出して、演出後に徐々に見えなくしてから破棄(見えなくなるまでの時間をコンボの継続時間と合わせるようにする)
        sequence.Append(imgCombo.transform.DOShakePosition(0.25f)).SetEase(Ease.Flash);
        sequence.Join(imgCombo.transform.DOScale(scale, 0.25f)).SetEase(Ease.Flash);
        sequence.AppendInterval(0.25f);
        sequence.Append(canvasGroup.DOFade(0f, 1.5f)).OnComplete(() => { Destroy(gameObject); });
    }
}
