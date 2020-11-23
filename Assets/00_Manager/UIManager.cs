using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] // Canvasの子である ComboObjTran ゲームオブジェクトをドラッグアンドドロップして、Transform の情報をアサインする
    private Transform comboObjTran;

    [SerializeField] // プレファブになっている ComboDetail ゲームオブジェクトをドラッグアンドドロップして、ComboDetail クラスの情報をアサインする
    private ComboDetail comboDetailPrefab;    

    public void CreateComboDetail(int comboCount)
    {
        // コンボ数表示を生成(ComboDetail クラスの持つメソッドを実行したいので、ComboDetail クラスで生成しておくことで GetComponent を実行しなくて済むようにする)
        ComboDetail comboDetail = Instantiate(comboDetailPrefab, comboObjTran, false);

        // コンボ数表示を設定したり、アニメ演出したりする処理を実行
        comboDetail.SetUpComboDetail(comboCount);
    }
}
