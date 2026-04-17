using System.Collections.Generic;
using UnityEngine;

public class TilePatterns
{
    private StageDataBase stDB;
    private List<int[,]> allPatterns = new List<int[,]>
    {
        // 0~4 maxPhase 1
        new int[,] {{0,1,0},{1,1,1},{0,1,0}},
        new int[,] {{1,1,0},{1,0,1},{0,1,1}},
        new int[,] {{0,1,0},{1,0,1},{0,1,0}},
        new int[,] {{0,0,1},{0,1,0},{1,0,0}},
        new int[,] {{1,1,1},{1,0,1},{1,1,1}},

        // 5~9 maxPhase 2
        new int[,] {{2,1,2},{1,0,1},{2,1,2}},
        new int[,] {{0,2,0},{2,2,2},{0,2,0}},
        new int[,] {{1,0,2},{0,0,0},{2,0,1}},
        new int[,] {{0,1,1},{2,0,1},{2,2,0}},
        new int[,] {{0,0,0},{1,1,1},{2,2,2}},

        // 10~14 maxPhase 3
        new int[,] {{3,0,3},{0,3,0},{3,0,3}},
        new int[,] {{3,3,0},{3,1,2},{0,2,2}},
        new int[,] {{0,1,1},{2,2,0},{0,3,3}},
        new int[,] {{2,3,1},{3,1,2},{1,2,3}},
        new int[,] {{1,1,3},{3,0,3},{3,1,1}},

        // 15~17 maxPhase 4
        new int[,] {{4,4,4},{4,4,4},{4,4,4}},
        new int[,] {{4,2,4},{2,0,2},{4,2,4}},
        new int[,] {{1,1,2},{4,0,2},{4,3,3}},
    };

    public TilePatterns(StageDataBase stageDataBase)
    {
        stDB = stageDataBase;
    }

    public int[,] GetPatternByLevel(int level)
    {
        level = Mathf.Clamp(level, 1, stDB.stageData.Length-1);
        int index = Random.Range(0, stDB.stageData[level].stageNum.Length);
        return allPatterns[stDB.stageData[level].stageNum[index]];
    }

    public int GetMaxCountByLevel(int level)
    {
        return stDB.stageData[level].maxCount;

    }

    public int GetMaxPhaseByLevel(int level)
    {
        return stDB.stageData[level].maxPhase;
    }
}
