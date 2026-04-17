using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ObservedData")]
public class ObservedData : ScriptableObject
{
    private int level = -1;
    private int count = -1;
    private int time = -1;
    public int life = -1;
    private bool isTurtorial = false;
    private bool isGameEnd = false;
    private bool isGameClear = false;
    public Action<int> OnLevelChanged;
    public Action<int> OnCountChanged;
    public Action<int> OnTimeChanged;
    public Action<int> OnLifeChanged;
    public Action<bool> OnIsTurtorialChanged;
    public Action<bool> OnIsGameEndChanged;
    public Action<bool> OnIsGameClearChanged;

    public void Init()
    {
        level = -1;
        count = -1;
        time = -1;
        life = -1;
        isTurtorial = false;
        isGameEnd = false;
        isGameClear = false;
    }

    public int Level
    {
        get => level;
        set
        {
            level = value;
            OnLevelChanged?.Invoke(level);
        }
    }

    public int Count
    {
        get => count;
        set
        {
            count = value;
            OnCountChanged?.Invoke(count);
        }
    }

    public int Time
    {
        get => time;
        set
        {
            time = value;
            OnTimeChanged?.Invoke(time);
        }
    }

    public int Life
    {
        get => life;
        set
        {
            life = value;
            OnLifeChanged?.Invoke(life);
        }
    }

    public bool IsTurtorial
    {
        get => isTurtorial;
        set
        {
            isTurtorial = value;
            OnIsTurtorialChanged?.Invoke(isTurtorial);
        }
    }

    public bool IsGameEnd
    {
        get => isGameEnd;
        set
        {
            isGameEnd = value;
            OnIsGameEndChanged?.Invoke(isGameEnd);
        }
    }

    public bool IsGameClear
    {
        get => isGameClear;
        set
        {
            isGameClear = value;
            OnIsGameClearChanged?.Invoke(isGameClear);
        }
    }
}
