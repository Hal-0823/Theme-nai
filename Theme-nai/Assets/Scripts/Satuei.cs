using UnityEngine;

public class Satuei : MonoBehaviour
{
    public Tile[] tiles;
    public Tile[] allTiles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            AllFlip();
        }
    }

    void Flip()
    {
        foreach(var tile in tiles)
        {
            tile.PlayFlipAnim();
        }
    }

    void AllFlip()
    {
        foreach(var tile in allTiles)
        {
            tile.PlayFlipAnim();
        }
    }
}
