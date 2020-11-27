using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] // Canvasの子である ComboObjTran ゲームオブジェクトをドラッグアンドドロップして、Transform の情報をアサインする
    private Transform comboObjTran;

    [SerializeField] // プレファブになっている ComboDetail ゲームオブジェクトをドラッグアンドドロップして、ComboDetail クラスの情報をアサインする
    private ComboDetail comboDetailPrefab;

    [SerializeField] 
    private Text infoText;

    float timeleft;
    int textIndex;


    public void CreateComboDetail(int comboCount)
    {
        // コンボ数表示を生成(ComboDetail クラスの持つメソッドを実行したいので、ComboDetail クラスで生成しておくことで GetComponent を実行しなくて済むようにする)
        ComboDetail comboDetail = Instantiate(comboDetailPrefab, comboObjTran, false);

        // コンボ数表示を設定したり、アニメ演出したりする処理を実行
        comboDetail.SetUpComboDetail(comboCount);
    }

    void InfoText()
    {
        string message = "";
        Debug.Log("空白");
    }

    void Start()
    {
        InfoText();
    }

    void TextIndex()
    {
        textIndex = Random.Range(1, 101);
        //Debug.Log(attackIndex);
    }


    void Update()
    {
        timeleft -= Time.deltaTime;
        if (timeleft <= 0.0)
        {
            timeleft = 15.0f;
            TextIndex();

            if (1 <= textIndex && textIndex <= 34)
            {
                string message = "すべての敵を殲滅せよ!!";
                infoText.DOText(message, message.Length * 0.3f);
                InfoText();
            }
            else if (35 <= textIndex && textIndex <= 67)
            {
                string message = "敵の青く光る攻撃はカウンターチャンス!!";
                infoText.DOText(message, message.Length * 0.3f);
            }
            else
            {
                string message = "ダウン攻撃、カウンター攻撃で体力回復!!";
                infoText.DOText(message, message.Length * 0.3f);
            }
        }
        if (timeleft <= 5.0f)
        {
            Debug.Log("空白");
            string message = "";
            infoText.DOText(message, message.Length * 0.3f);

        }

    }
}
