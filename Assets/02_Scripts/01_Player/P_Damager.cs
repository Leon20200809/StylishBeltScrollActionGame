using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class P_Damager : MonoBehaviour
{
    public int atkPow_L;
    public int atkPow_Kick;
    public int atkPow_H;
    public int atkPow_I;
    public int atkPow_Z;
    public int atkPow_Kumiuchi;
    public int atkPow_Oiuchi;
    Collider wcollider;
    public PlayerController playerController;

    public int hitCount;
    public float comboTime;
    public Text combCountText;
    public bool isCombo;

    [Header("コンボの受付時間")]
    public float comboLimit;

    [SerializeField]
    private int comboCount;            // 現在のコンボ数(インスペクターで確認したい場合には[SerializeField]属性をつけてください)
    [SerializeField]
    private float comboLimitTimer;     // コンボの受付時間用のタイマー
    [SerializeField]
    private bool isComboChain;         // コンボ中かどうかの判定用。true はコンボ中。false は非コンボ中(インスペクターで確認したい場合には[SerializeField]属性をつけてください)

    [SerializeField]
    private UIManager uiManager;       // UIManager 代入用。ヒエラルキーにある UIManager ゲームオブジェクトをアサインして UIManager スクリプトを取得



    private void OnTriggerEnter(Collider other)
    {
        //タグ判定
        if (other.CompareTag("Enemy") || other.CompareTag("BossEnemy"))
        {
            wcollider.enabled = false;
            TriggerComboCount();
            //ComboCountStart();
            Debug.Log("プレイヤー攻撃HIT");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        wcollider = GetComponent<Collider>();
        atkPow_L = GameData.instance.charaDataList[0].baseAttackPower;
        atkPow_Kick = Mathf.RoundToInt(atkPow_L * 0.3f);
        atkPow_H = Mathf.RoundToInt(atkPow_L * 1.4f);
        atkPow_I = atkPow_L * 3;
        atkPow_Z = Mathf.RoundToInt(atkPow_L * 1.3f);
        atkPow_Kumiuchi = atkPow_L * 7;
        atkPow_Oiuchi = atkPow_L * 3;
    }

    // Update is called once per frame
    void Update()
    {
        // コンボの時間判定
        UpdateComboLimitTime();

        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Z))
        {    // <=　好きなキーを登録する

            // TODO 実際にはこのメソッドは攻撃ヒット時に呼び出すようにする
            TriggerComboCount();
        }

        if (isCombo)
        {
            comboTime += Time.deltaTime;
            if (comboTime > 2f)
            {
                isCombo = false;
                comboTime = 0f;
                hitCount = 0;
                combCountText.text = "";
                Sequence sequence = DOTween.Sequence();
            }

        }
    }


    public void ComboCountStart()
    {
        isCombo = true;
        hitCount++;
        comboTime = 0;
        combCountText.text = hitCount + " Hit!";
    }

    /// <summary>
    /// コンボ処理の際に呼ばれる
    /// </summary>
    public void TriggerComboCount()
    {
        // 攻撃時に呼ばれる　コンボ中に設定
        isComboChain = true;

        // コンボのカウントを加算
        comboCount++;

        // タイマーをリセット
        comboLimitTimer = 0;

        // コンボ数表示を生成
        uiManager.CreateComboDetail(comboCount);

        Debug.Log("コンボ中 : " + comboCount + " Hit!");
    }

    /// <summary>
    /// コンボの持続時間を計測(Updateメソッドで常に呼ばれている)
    /// </summary>
    private void UpdateComboLimitTime()
    {

        // コンボ中でなければタイマーは計測しない(Updateメソッド内で呼ばれているため、必要時(コンボ時)以外には処理を行わないようにする制御が必要)
        if (!isComboChain)
        {
            return;
        }

        // タイマー計測
        comboLimitTimer += Time.deltaTime;

        // タイマーの計測値がコンボ持続時間の規定値を超えたら
        if (comboLimitTimer >= comboLimit)
        {
            // コンボ終了
            isComboChain = false;

            // 設定を初期化
            comboLimitTimer = 0;
            comboCount = 0;

            Debug.Log("コンボ終了");
        }
    }
}
