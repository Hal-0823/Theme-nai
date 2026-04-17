using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData")]
public class StageDataBase : ScriptableObject
{
    public StageData[] stageData;   // レベル順
}

[System.Serializable]
public class StageData
{
    public int maxCount = 1;
    public int maxPhase = 1;
    public int[] stageNum;
}

