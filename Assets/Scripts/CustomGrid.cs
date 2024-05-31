using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteSet
{
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
}

public class CustomGrid : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Tile tile;
    [SerializeField] private Camera myCamera;
    [SerializeField] private SpriteSet[] spriteSets;
    [SerializeField] private Vector3[] bulletPositions;
    [SerializeField] private SpriteRenderer currentSprite;
    [SerializeField] private Bullet bullet;
    public int Width => width;

    private void Awake()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float scaleValue = tile.transform.localScale.x;
        int i = 0;
        float totalOffset = 0.5f - (0.5f * scaleValue);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile spawnedTile = Instantiate(tile, new Vector2(x * scaleValue, y * scaleValue), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset, spriteSets[i], currentSprite, bullet, bulletPositions[i]);
                i++;
            }
        }

        myCamera.transform.position = new Vector3((float)width / 2 - totalOffset, (float)height / 2 - totalOffset, -10);
    }
}
