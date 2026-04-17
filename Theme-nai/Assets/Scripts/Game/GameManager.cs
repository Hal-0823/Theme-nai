using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using unityroom.Api;

public class GameManager : MonoBehaviour
{
    private ObservedData obData;
    private int gridSize;
    private Tile[,] aTiles; // プレイヤーが操作するタイルパターン 
    private Tile[,] qTiles; // 出題されるタイルパターン
    private TilePatterns tilePatterns;
    private int[,] pattern;
    private int maxCount;
    private int maxPhase;
    private List<Id> clickOrderList;

    public GameManager(ObservedData observedData, StageDataBase stageDataBase, int size, GameObject tileParentPref, Tile tilePref)
    {
        this.obData = observedData;
        obData.Level = obData.IsTurtorial ? 0 : 1;
        tilePatterns = new TilePatterns(stageDataBase);
        gridSize = size;
        aTiles = new Tile[size, size];
        qTiles = new Tile[size, size];
        GridSpawner gridSpawner = new GridSpawner(tilePref, tileParentPref, size);
        
        gridSpawner.SpawnGrid(aTiles);
        foreach (var tile in aTiles)
        {
            tile.EnableClick(true);
            tile.OnClickTile += TileClickedEvent;
            tile.ChangeColor(); // 初期色の反映
        }

        gridSpawner.SpawnGrid(qTiles);
        foreach (var tile in qTiles)
        {
            tile.ChangeColor(); // 初期色の反映
        }
        AllTileEnableClick(qTiles, false);

        // 出題タイルのサイズと位置を調整する
        qTiles[0,0].transform.parent.transform.Translate(-5,0,0);
        qTiles[0,0].transform.parent.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        SetTilePattern(obData.Level);

        /*
        Timer timer = new Timer(obData, 100);
        timer.OnTimerFinished += GameEnd;
        timer.StartTimer();
        */

        obData.Life = 3;
    }

    // タイルパターンを生成する
    public void SetTilePattern(int level)
    {
        // クリック順のリセット
        clickOrderList = new List<Id>();

        pattern = tilePatterns.GetPatternByLevel(level);
        maxCount = tilePatterns.GetMaxCountByLevel(level);
        maxPhase = tilePatterns.GetMaxPhaseByLevel(level);

        Debug.Log("Count call");
        obData.Count = maxCount;
        Debug.Log(obData.Count);

        for (int x=0; x<gridSize; x++)
        {
            for (int y=0; y<gridSize; y++)
            {
                qTiles[x,y].SetPhase(pattern[x,y]);
                qTiles[x,y].SetMaxPhase(maxPhase);
                aTiles[x,y].SetPhase(pattern[x,y]);
                aTiles[x,y].SetMaxPhase(maxPhase);
            }
        }

        for (int count=maxCount; count>0; count--)
        {
            int x = 0;
            int y = 0;

            if (obData.IsTurtorial)
            {
                x = 1;
                y = 1;
                obData.IsTurtorial = false;
            }
            else
            {
                x = UnityEngine.Random.Range(0, gridSize);
                y = UnityEngine.Random.Range(0, gridSize);
            }
            

            aTiles[x,y].RerverseFlip();

            for (int i=1; i<2; i++)
            {
                if (IsInGrid(x + i)) aTiles[x + i, y].RerverseFlip();
                if (IsInGrid(x - i)) aTiles[x - i, y].RerverseFlip();
                if (IsInGrid(y + i)) aTiles[x, y + i].RerverseFlip();
                if (IsInGrid(y - i)) aTiles[x, y - i].RerverseFlip();
            }
        }

        // 出題したパターンと被ってしまった場合もう一度作り直す
        if (Judge())
        {
            SetTilePattern(level);
            return;
        }
        foreach (var tile in qTiles)
        {
            tile.PlayFlipAnim(false);
        }

        foreach (var tile in aTiles)
        {
            tile.PlayFlipAnim(false);
        }

        AllTileEnableClick(aTiles, true);
    }

    public async void TileClickedEvent(Id id)
    {
        // gameEndしていたらこれ以降の処理を行わない
        if (obData.IsGameEnd) return;

        AudioManager.Instance.PlaySE(SEData.SEName.TileClick);

        Debug.Log("click");
        // クリック順に格納
        clickOrderList.Add(id);
        obData.Count = maxCount - clickOrderList.Count;

        AllTileEnableClick(aTiles, false);

        //aTiles[id.x, id.y].PlayClickAnim();
        aTiles[id.x, id.y].PlayFlipAnim();

        //await UniTask.Delay(333);

        for (int i=1; i<2; i++)
        {
            if (IsInGrid(id.x + i)) aTiles[id.x + i, id.y].PlayFlipAnim();
            if (IsInGrid(id.x - i)) aTiles[id.x - i, id.y].PlayFlipAnim();
            if (IsInGrid(id.y + i)) aTiles[id.x, id.y + i].PlayFlipAnim();
            if (IsInGrid(id.y - i)) aTiles[id.x, id.y - i].PlayFlipAnim();
        }

        await UniTask.Delay(850);

        // gameEndしていたらこれ以降の処理を行わない
        if (obData.IsGameEnd) return;

        Debug.Log("Game continue");
        if (obData.Count <= 0)
        {
            await UniTask.Delay(550);
            if(Judge()) 
            {
                OnCorrectAnswer();
                return;
            }
            else
            {
                await OnIncorrectAnswer();
            }
        }

        AllTileEnableClick(aTiles, true);
    }

    private bool IsInGrid(int id)
    {
        if (id < gridSize && id >= 0)
        {
            return true;
        }
        return false;
    }

    private void AllTileEnableClick(Tile[,] tiles, bool click)
    {
        foreach (var tile in tiles)
        {
            tile.EnableClick(click);
        }
    }

    private bool Judge()
    {
        for (int x=0; x<gridSize; x++)
        {
            for (int y=0; y<gridSize; y++)
            {
                if (qTiles[x,y].GetPhase() != aTiles[x,y].GetPhase())
                {
                    return false;
                }
            }
        }
        return true;
    }

    private async void OnCorrectAnswer()
    {
        obData.Level++;
        obData.Life = 3;
        foreach (var tile in aTiles)
        {
            tile.PlaySuccessAnim();
        }

        AudioManager.Instance.PlaySE(SEData.SEName.Correct);

        await UniTask.Delay(1000);

        if (obData.Level < 32)
        {
            SetTilePattern(obData.Level);
        }
        else
        {
            GameClear();
        }
    }

    private async UniTask OnIncorrectAnswer()
    {
        obData.Life--;
        AudioManager.Instance.PlaySE(SEData.SEName.Incorrect);

        while (clickOrderList.Count > 0)
        {
            Id id = clickOrderList[clickOrderList.Count-1];
            clickOrderList.RemoveAt(clickOrderList.Count-1);

            aTiles[id.x, id.y].PlayReverseFlipAnim();

            for (int i=1; i<2; i++)
            {
                if (IsInGrid(id.x + i)) aTiles[id.x + i, id.y].PlayReverseFlipAnim();
                if (IsInGrid(id.x - i)) aTiles[id.x - i, id.y].PlayReverseFlipAnim();
                if (IsInGrid(id.y + i)) aTiles[id.x, id.y + i].PlayReverseFlipAnim();
                if (IsInGrid(id.y - i)) aTiles[id.x, id.y - i].PlayReverseFlipAnim();
            }

            await UniTask.Delay(700);
        }
        obData.Count = maxCount;
        if (obData.Life <= 0)
        {
            GameEnd();
        }
    }

    public async void GameEnd()
    {
        // ランキング登録（ハイスコア更新）
        UnityroomApiClient.Instance.SendScore(1, obData.Level, ScoreboardWriteMode.HighScoreDesc);

        obData.IsGameEnd = true;
        AllTileEnableClick(aTiles, false);
        AudioManager.Instance.PlaySE(SEData.SEName.Fall);
        aTiles[0,0].transform.parent.GetComponent<Animator>().SetTrigger("Fall");
        qTiles[0,0].transform.parent.GetComponent<Animator>().SetTrigger("Fall");

        
    }

    public async void GameClear()
    {
        // ランキング登録（ハイスコア更新）
        UnityroomApiClient.Instance.SendScore(1, obData.Level, ScoreboardWriteMode.HighScoreDesc);
        
        AudioManager.Instance.StopBGM();
        await UniTask.Delay(2000);
        AudioManager.Instance.PlayBGM(BGMData.BGMName.Ending);
        await UniTask.Delay(3000);
        obData.IsGameClear = true;
        AllTileEnableClick(aTiles, false);
    }

    public void Dispose()
    {
        foreach (var tile in aTiles)
        {
            Destroy(tile.gameObject);
        }

        foreach (var tile in qTiles)
        {
            Destroy(tile.gameObject);
        }
        
        Destroy(this);
    }
}
