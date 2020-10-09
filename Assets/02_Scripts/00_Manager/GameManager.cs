using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //スクリプタブル・オブジェクトを登録
    public StageList stageList;

    //現在のエリア数。敵をすべて倒すとカウントアップ
    public int areaIndex;

    //現在のステージ数。ボスを倒してクリアすると増える。GameDataに管理させる
    public int currentStageNo;

    public PlayerController playerController;

    // 現在のエリアの移動制限の各値(確認用。不要になったら削除する) 
    public float leftLimitPos;
    public float rightLimitPos;
    public float forwordLimitPos;
    public float backLimitPos;

    //現在のステージのデータ
    public StageList.StageData currentStageData;

    // Start is called before the first frame update
    void Start()
    {
        InitStage();
        SetUpNextArea();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ステージの番号を取得してステージの準備を行う
    /// </summary>
    private void InitStage()
    {
        // TODO GameDataからステージ番号をセット。現在はステージ１としておく
        currentStageNo = 0;

        // ステージ番号から、どのステージかを検索してStageDataを取得　①か②のどちらかを使う。
        // ①Findメソッド　
        currentStageData = stageList.stageDatas.Find(x => x.stageNo == currentStageNo);

        // ②foreach
        //foreach (StageList.StageData stageData in stageList.stageDatas) 
        //{
        //    if (stage.stageNo == currentStageNo)
        //    {
        //        currentStageData = stageData;
        //    }
        //}

        // エリアの番号をセット
        areaIndex = 0;
    }

    /// <summary>
    /// エリア番号からエリアの情報を取得
    /// </summary>
    private void SetUpNextArea()
    {
        // エリア番号からエリアの情報を取得

        // 移動範囲制限
        leftLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.horizontalLimit.left;
        rightLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.horizontalLimit.right;
        forwordLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.depthLimit.forword;
        backLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.depthLimit.back;

        // TODO 敵の生成処理

    }
}
