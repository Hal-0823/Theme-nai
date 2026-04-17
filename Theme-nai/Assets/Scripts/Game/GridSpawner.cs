using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    private Tile prefab; // 配置するタイル
    private GameObject parentPrefab; // 配置するタイルの親プレハブ
    private int gridSize; // 配置グリッドのサイズ
    public float spacing = 1.5f; // タイル間の間隔

    public GridSpawner(Tile prefab, GameObject parentPrefab, int gridSize)
    {
        this.prefab = prefab;
        this.parentPrefab = parentPrefab;
        this.gridSize = gridSize;
    }

    public void SpawnGrid(Tile[,] array)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned!");
            return;
        }

        float offset = (gridSize - 1) * spacing / 2.0f; // 中心を(0,0)にするためのオフセット

        GameObject parent = Instantiate(parentPrefab, new Vector2(0,0), Quaternion.identity);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // 配置位置を計算
                float posX = x * spacing - offset;
                float posY = y * spacing - offset;

                // オブジェクトを生成
                Vector3 position = new Vector3(posX, posY, 0);
                array[x,y] = Instantiate(prefab, position, Quaternion.identity, parent.transform);
                array[x,y].SetIndex(x,y);
            }
        }
    }
}

