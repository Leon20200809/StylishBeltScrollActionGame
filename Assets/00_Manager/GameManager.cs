using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("オンで敵生成しない")]
    public bool isDebug;

    //スクリプタブル・オブジェクトを登録
    public StageList stageList;

    //現在のエリア数。敵をすべて倒すとカウントアップ
    public int areaIndex;

    //現在のステージ数。ボスを倒してクリアすると増える。GameDataに管理させる
    public int currentStageNo;

    //プレイヤーゲームオブジェクト
    public PlayerController playerController;
    public PlayerController playerController_Prefab;
    public CameraFollow cameraFollow;


    // 現在のエリアの移動制限の各値(確認用。不要になったら削除する) 
    public float leftLimitPos;
    public float rightLimitPos;
    public float forwordLimitPos;
    public float backLimitPos;

    //現在のステージのデータ
    public StageList.StageData currentStageData;

    //エリア内で生成した敵の数の合計値
    public int generateCount;

    //生成間隔カウント用
    public float generaterTimer;

    //待機時間
    public int appearTime;

    //生成する敵のプレファブ。GameObject型ではなく、Enemy型(スクリプトの名前)でアサインする
    public List<EnemyController> enemyPrefabs = new List<EnemyController>();

    //敵のHPUI用
    public Transform enemyHPSliderGenerateTran;
    //敵のHPUI用
    public Transform bossEnemyHPSliderGenerateTran;

    //生成した敵を入れるリスト
    public List<EnemyController> enemiesList = new List<EnemyController>();

    //このエリアで倒した敵の数
    public int destroyCount;

    //エリア内敵、生成完了フラグ trueで生成完了
    public bool isCompleteGenerate;

    //
    public GameObject limitObjPrefab;
    GameObject limitObj;

    [SerializeField] //Transform の情報をアサインする
    private Transform goAheadObjTran;

    [SerializeField] //ComboDetail クラスの情報をアサインする
    private GoAheadDetail goAheadObjTranPrefab;

    //ゲーム状況
    public enum GameState
    {
        Play,
        Wait,
        Move,
    }
    public GameState gameState = GameState.Wait;

    // Start is called before the first frame update
    void Start()
    {
        InitStage();
        Generate_Player();
        SetUpNextArea();
    }

    private void Generate_Player()
    {
        playerController = Instantiate(playerController_Prefab);
        playerController.InitPlayer(this);
        cameraFollow.Setup_Camera(playerController.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDebug == true)
        {
            generateCount = 0;
            return;
        }

        if (gameState == GameState.Wait)
        {
            return;
        }

        if (gameState == GameState.Move)
        {
            // エリア移動可能な状態のときに、キャラの位置がエリアの右端を超えたら次のエリアへ移る
            if (playerController.transform.position.x >= rightLimitPos)
            {
                // 次のエリア設定のためにGameStateをWaitにしてUpdateメソッドを動かないようにする
                gameState = GameState.Wait;
                Debug.Log(gameState);

                // エリアの番号を進める
                areaIndex++;
                if (areaIndex == 2)
                {
                    SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Game2);
                }
                if (areaIndex == 3)
                {
                    SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Game3);
                }
                

                //ステージをクリアしたか確認
                if (CheckStageClear())
                {
                    NextStage();
                }
                else
                {
                    // 次のエリアをセット
                    SetUpNextArea();
                }
            }
        }

        //用意してあるエリア数を超えたら処理しない
        if (areaIndex >= currentStageData.areaDatas.Count)
        {
            return;
        }

        //生成数チェック
        if (generateCount >= currentStageData.areaDatas[areaIndex].appearNum.Length)
        {
            return;
        }

        //敵生成カウントタイマー作動
        generaterTimer += Time.deltaTime;
        if (generaterTimer >= appearTime)
        {
            Debug.Log("敵出現");
            GenerateEnemy(playerController.transform.position, generateCount);
            generaterTimer = 0;
        }
    }

    /// <summary>
    /// ステージの番号を取得してステージの準備を行う
    /// </summary>
    private void InitStage()
    {
        // gameState をWaitに設定してUpdateメソッドを止めておく
        gameState = GameState.Wait;
        Debug.Log(gameState);

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
        //エリア番号からエリアの情報を取得

        //移動範囲制限
        leftLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.horizontalLimit.left;
        rightLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.horizontalLimit.right;
        forwordLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.depthLimit.forword;
        backLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.depthLimit.back;

        //敵の生成数初期化
        generateCount = 0;

        //敵の生成間隔 5秒～9秒
        appearTime = Random.Range(5, 10);

        //敵の討伐数を初期化
        destroyCount = 0;

        //敵の生成リスト初期化
        enemiesList.Clear();

        //エリア内の敵生成完了フラグリセット
        isCompleteGenerate = false;

        //移動範囲制限可視化
        Area_Limit_Generation();

        //ゲームスタート
        gameState = GameState.Play;
        Debug.Log(gameState);

    }

    void Area_Limit_Generation()
    {
        int v = (int)(forwordLimitPos - backLimitPos);
        int h = (int)(rightLimitPos - leftLimitPos);
        Debug.Log(v);
        Debug.Log(h);
        for (int i = 0; i < v ; i++)
        {
            limitObj = Instantiate(limitObjPrefab, new Vector3(leftLimitPos - 1, 1, backLimitPos + i), Quaternion.Euler(0, 0, 0));
        }
        for (int i = 0; i < v ; i++)
        {
            limitObj = Instantiate(limitObjPrefab, new Vector3(rightLimitPos, 1, backLimitPos + i), Quaternion.Euler(0, 0, 0));
        }
        /*for (int i = 1; i < h ; i++)
        {
            GameObject leftLimitforword = Instantiate(limitObjPrefab, new Vector3(leftLimitPos + i, 1, forwordLimitPos), Quaternion.Euler(0, 0, 0));
        }
        for (int i = 1; i < h ; i++)
        {
            GameObject leftLimitforword = Instantiate(limitObjPrefab, new Vector3(leftLimitPos + i, 1, backLimitPos), Quaternion.Euler(0, 0, 0));
        }*/
    }

    void GenerateEnemy(Vector3 charaPos, int enemyIndex)
    {
        //位置ランダム生成
        int direction = Random.Range(0, 2);
        charaPos.x = direction == 0 ? charaPos.x += 5f : charaPos.x -= 5f;
        float posX = charaPos.x;
        float posZ = charaPos.z;
        Vector3 generatePos = new Vector3(posX + Random.Range(-0.5f, 0.5f), charaPos.y, Random.Range(-0.5f, 0.5f));

        //StageListに登録した敵の情報を元に敵を生成。生成する際にEnemy型のプレファブを使用するので、左辺に用意する変数の型もEnemy型としている
        EnemyController enemy = Instantiate(enemyPrefabs[currentStageData.areaDatas[areaIndex].appearNum[enemyIndex]], generatePos, Quaternion.identity);

        //Enemy型で生成して変数に代入しているので、すぐにメソッドの呼び出しができる
        enemy.SetUpEnemy(this);

        //生成した敵をリストに追加
        enemiesList.Add(enemy);

        //生成数を加算
        generateCount++;

        //生成上限チェック
        if (generateCount >= currentStageData.areaDatas[areaIndex].appearNum.Length)
        {
            isCompleteGenerate = true;
        }
        else
        {
            //次に敵が出現するまでの時間を設定
            appearTime = Random.Range(5, 10);
        }
    }

    /// <summary>
    /// 討伐数を加算してenemyListから倒した敵を削除
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemyList(EnemyController enemy)
    {
        destroyCount++;
        enemiesList.Remove(enemy);
        CheckAreaClear();
    }

    void Area_Limit_Destory()
    {
        GameObject[] del = GameObject.FindGameObjectsWithTag("LimitObj");

        foreach(GameObject del_Index in del)
        {
            Destroy(del_Index);
        }
    }

    /// <summary>
    /// GO→演出
    /// </summary>
    void Create_Area_Go_Ahead()
    {
        GoAheadDetail goAheadObj = Instantiate(goAheadObjTranPrefab, goAheadObjTran, false);
        goAheadObj.SetUpGoAheadDetail();
    }


    /// <summary>
    /// エリア内の敵の生成状況と討伐状況を確認してエリアクリアかどうか判定
    /// </summary>
    void CheckAreaClear()
    {
        //生成が完了しないとクリアできない
        if (!isCompleteGenerate)
        {
            return;
        }

        if (destroyCount >= generateCount)
        {
            Debug.Log("エリアクリア");
            // 次のエリアへ移動可能にする
            gameState = GameState.Move;
            Debug.Log(gameState);

            //リミットオブジェクト削除
            Area_Limit_Destory();

            //→GO演出
            Create_Area_Go_Ahead();
        }
    }

    /// <summary>
    /// ステージクリア判定
    /// </summary>
    /// <returns></returns>
    bool CheckStageClear()
    {
        if (areaIndex >= currentStageData.areaDatas.Count)
        {
            return true;
        }
        return false;
    }

    void NextStage()
    {
        //ステージ番号を次に送る
        currentStageNo++;

        //すべてのステージをクリアしたか確認する
        if (currentStageNo >= stageList.stageDatas.Count)
        {
            //
            Debug.Log("ゲームクリア");
            EscMenu.GoTitleButton();
        }
        else
        {
            //次のステージへ
            Debug.Log("次のステージへ");

            // TODO ステージクリア演出
            

            //次のステージのデータを取得
            InitStage();

            //次のステージの最初のエリアを用意
            SetUpNextArea();
        }
    }
}
