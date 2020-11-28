using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class StartButton : MonoBehaviour
{
    public Button startButton;
    public Transform startButtonTran;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(GameStart);
    }

    void GameStart()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.SE_16);
        //DoTween 大きさを1.5倍に0.1秒かけて0.1秒維持して元に戻す
        Sequence sequence = DOTween.Sequence();
        sequence.Append(startButtonTran.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.1f));
        sequence.AppendInterval(0.1f);
        sequence.Append(startButtonTran.DOScale(Vector3.one, 0.1f));
        StartCoroutine(Kankaku());
    }

    IEnumerator Kankaku() //コルーチンメソッド変数名Kankaku
    {
        //(1.5秒の間を設ける)
        yield return new WaitForSeconds(1f);
        Debug.Log("Wait");

        //ゲームシーンへ移行
        SceneManager.LoadScene("GameScene");
    }

}
