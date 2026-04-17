using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Timer : MonoBehaviour
{
    private ObservedData obData;
    private int timeLimit;
    private float time;
    public Action OnTimerFinished;

    public Timer(ObservedData observedData, int timeLimit)
    {
        this.obData = observedData;
        this.timeLimit = timeLimit;
    }

    public void StartTimer()
    {
        time = 0;
        CountDown();
    }

    private async UniTask CountDown()
    {
        int preTime = 0;
        obData.Time = timeLimit;

        while ((int)time < timeLimit)
        {
            await UniTask.Yield();
            time += Time.deltaTime;
            if (preTime != (int)time)
            {
                obData.Time = timeLimit - (int)time;
            }
            preTime = (int)time;
        }
        Debug.Log("Time's up!");
        OnTimerFinished?.Invoke();
    }
}
