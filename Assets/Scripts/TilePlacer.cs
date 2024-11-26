using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacer : MonoBehaviour
{
    Tilemap tilemap;
    public TileBase tile;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }
    void placeTile(Vector3Int cellPosition)
    {
        tilemap.SetTile(cellPosition, tile);
    }

    public void placeTile(Vector3 position)
    {
        placeTile(tilemap.WorldToCell(position));
    }

    void removeTile(Vector3Int cellPosition)
    {
        tilemap.SetTile(cellPosition, null);
    }

    public void removeTile(Vector3 position)
    {
        removeTile(tilemap.WorldToCell(position));
    }

    public void FillCircleWithTiles(Vector2 center, float radius, bool fill = true)
    {
        for (int x = -Mathf.FloorToInt(radius); x <= Mathf.FloorToInt(radius); x++)
        {
            for (int y = -Mathf.FloorToInt(radius); y <= Mathf.FloorToInt(radius); y++)
            {
                Vector2 position = new Vector2(center.x + x, center.y + y);
                if (Vector2.Distance(center, position) <= radius)
                {
                    if (fill)
                        placeTile(position);
                    else
                        removeTile(position);
                }
            }
        }
    }

    public void FillSquareWithTiles(Vector2 center, float radius, bool fill = true)
    {
        for (int x = -Mathf.FloorToInt(radius); x <= Mathf.FloorToInt(radius); x++)
        {
            for (int y = -Mathf.FloorToInt(radius); y <= Mathf.FloorToInt(radius); y++)
            {
                Vector2 position = new Vector2(center.x + x, center.y + y);
                if (fill)
                    placeTile(position);
                else
                    removeTile(position);
            }
        }
    }

}
