using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("デバッグモード チェックでオン")]
    public bool isDebug;

    GameManager gameManager;

    /// <summary>
    /// EnemyController初期設定
    /// </summary>
    /// <param name="gameManager">GameManagerクラスから引数として引き入れ</param>
    public void SetUpEnemy(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    // Start is called before the first frame update
    void Start()
    {
        //isDebugがTrueの場合
        if (isDebug)
        {
            StartCoroutine(DestroyEnemy(3.0f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 敵オブジェクト削除
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    public IEnumerator DestroyEnemy(float waitTime = 0.0f)
    {
        Debug.Log("敵撃破処理");
        yield return new WaitForSeconds(waitTime);
        gameManager.RemoveEnemyList(this);
        Destroy(gameObject);
    }

    public void DestroyEnemySend()
    {
        //gameManager.RemoveEnemyList(this);
        Destroy(gameObject);
    }
}

