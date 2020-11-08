using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public int hitCount;
    public float comboTime;
    public Text combCountText;

    public int scoreCount;


    [System.Serializable]
    public class CharaData
    {
        public int no;
        public int hp;
        public float moveSpeed;
        public float rotateSpeed;
        public int attackPower;
    }

    public List<CharaData> charaDataList = new List<CharaData>();
    public CharaData currentPlayerData;
    public int currentStageNo;
    public int battleTime;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    public void ComboCountStart()
    {
        hitCount++;
        comboTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        comboTime += Time.deltaTime;
        if (comboTime > 2f)
        {
            comboTime = 0f;
            hitCount = 0;
        }

        combCountText.text = hitCount.ToString();
    }
}
