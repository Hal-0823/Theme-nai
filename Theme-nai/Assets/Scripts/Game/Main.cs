using UnityEngine;
using Cysharp.Threading.Tasks;

public class Main : MonoBehaviour
{
    public ObservedData observedData;
    public StageDataBase stageDataBase;
    [SerializeField] private GameObject tileParentPref;
    [SerializeField] private Tile tilePref;
    [SerializeField] private int gridSize;

    private GameManager gameManager;
    void Start()
    {
        observedData.Init();
        observedData.IsTurtorial = true;
        gameManager = new GameManager(observedData, stageDataBase, gridSize, tileParentPref, tilePref);
        AudioManager.Instance.PlayBGM(BGMData.BGMName.InGame);
    }

    public async void OnClickRetry()
    {
        AudioManager.Instance.PlaySE(SEData.SEName.Click);
        gameManager.Dispose();
        observedData.Init();

        await UniTask.Delay(1500);

        AudioManager.Instance.StopBGM();
        gameManager = new GameManager(observedData, stageDataBase, gridSize, tileParentPref, tilePref);
        AudioManager.Instance.PlayBGM(BGMData.BGMName.InGame);
    }
}
