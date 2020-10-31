using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "StageList", menuName = "Create StageList")]

public class StageList : ScriptableObject
{
    public List<StageData> stageDatas = new List<StageData>();

    [Serializable]
    public class StageData
    {
        public int stageNo;
        public List<AreaData> areaDatas = new List<AreaData>();
    }

    [Serializable]
    public class AreaData
    {
        public int areaNo;
        // エリアごとの移動制限範囲
        public MoveLimit areaMoveLimit;
        // 敵の番号を入れておく
        public int[] appearNum;                 
    }
}
