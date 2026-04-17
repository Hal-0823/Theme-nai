using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cysharp.Threading.Tasks;
using TMPro;

public class UIManager : MonoBehaviour
{
    public ObservedData observedData;
    public Volume volume;
    public GameObject cursor;
    public GameObject dialogue;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI timeText;
    public Tile_Canvas retryTile;
    public Tile_Canvas[] lifeTile;
    public Tile_Canvas[] gameclearTile;
    private Vignette vignette;
    private Bloom bloom;

    private void OnEnable()
    {
        observedData.OnLevelChanged += changeLevelText;
        observedData.OnCountChanged += changeCountText;
        observedData.OnTimeChanged += changeTimeText;
        observedData.OnLifeChanged += changeLifeTile;
        observedData.OnIsGameEndChanged +=changeIsGameEnd;
        observedData.OnIsGameClearChanged += changeIsGameClear;
        observedData.OnIsTurtorialChanged += changeIsTurtorial;
        volume.profile.TryGet(out vignette);
        if (vignette == null)
        {
            Debug.LogError("No vignette!");
        }
        volume.profile.TryGet(out bloom);
        if (bloom == null)
        {
            Debug.LogError("No bloom!");
        }
    }

    private void OnDisable()
    {
        observedData.OnLevelChanged -= changeLevelText;
        observedData.OnCountChanged -= changeCountText;
        observedData.OnTimeChanged -= changeTimeText;
        observedData.OnLifeChanged -= changeLifeTile;
        observedData.OnIsGameEndChanged -=changeIsGameEnd;
        observedData.OnIsGameClearChanged -= changeIsGameClear;
        observedData.OnIsTurtorialChanged -= changeIsTurtorial;
    }

    public async void changeLevelText(int level)
    {
        levelText.text = "Lv" + level;
        if (level > 0)
        {
            cursor.SetActive(false);
        }
        else
        {
            await UniTask.Delay(800);
            cursor.SetActive(true);
        }
    }

    public void changeCountText(int count)
    {
        dialogueText.text = "<color=#F11>" + count + "</color>" + "picks left...";
    }

    public void changeLifeTile(int life)
    {
        if (life > lifeTile.Length)
        {
            life = lifeTile.Length;
        }
        else if (life < 0)
        {
            life = 0;
        }

        for (int i=0; i<lifeTile.Length; i++)
        {
            Debug.Log(i);
            if (i < life)
            {
                if (lifeTile[i].GetPhase() != 1)
                {
                    lifeTile[i].SetPhase(1);
                    lifeTile[i].PlayFlipAnim();
                }
            }
            else
            {
                if (lifeTile[i].GetPhase() != 0)
                {
                    lifeTile[i].SetPhase(0);
                    lifeTile[i].PlayReverseFlipAnim();
                }
            }
        }

        if (life > 0)
        {
            vignette.smoothness.value = 0.06f + 0.01f * (lifeTile.Length - life);
        }
        
    }

    public void changeTimeText(int time)
    {
        timeText.text = time.ToString();
    }

    public void changeIsGameEnd(bool isGameEnd)
    {
        if (!isGameEnd) return;
        //dialogueText.text = "<color=#F11>" + "Time's up!" + "</color>";
        dialogueText.text = "<color=#F11>" + "GAME OVER!" + "</color>";
        retryTile.gameObject.SetActive(true);
        retryTile.PlayAppearanceAnim();
    }

    public async void changeIsGameClear(bool isGameClear)
    {
        if (!isGameClear) return;

        foreach (var tile in gameclearTile)
        {
            tile.gameObject.SetActive(true);
            bloom.threshold.value -= 0.05f;
            await UniTask.Delay(700);
            tile.PlayAppearanceAnim();
        }
        await UniTask.Delay(700);
        dialogueText.text = "<color=#FF19A2>" + "Thank you for playing!" + "</color>";
        
    }

    public async void changeIsTurtorial(bool isTurtorial)
    {
        
    }

    public async void OnClickRetry()
    {
        retryTile.PlayDisappearanceAnim();
        await UniTask.Delay(500);
        retryTile.gameObject.SetActive(false);
    }
}
