using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacer : MonoBehaviour
{
    Tilemap tilemap;
    public TileBase tile;
    float tileSize = 1f;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tileSize = transform.parent.GetComponent<Grid>().cellSize.x;
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
        float radiusInTiles = radius * tileSize;

        //draw the radius in tiles
        Debug.DrawLine(center, center + new Vector2(0, radiusInTiles), Color.blue, 2.0f);
        
        int diameterInTiles = Mathf.CeilToInt(radiusInTiles * 2);

        //draw bounding box
        Debug.DrawLine(center + new Vector2(-radiusInTiles, -radiusInTiles), 
                       center + new Vector2(radiusInTiles, -radiusInTiles), Color.green, 2.0f);
        Debug.DrawLine(center + new Vector2(radiusInTiles, -radiusInTiles),
                       center + new Vector2(radiusInTiles, radiusInTiles), Color.green, 2.0f);
        Debug.DrawLine(center + new Vector2(radiusInTiles, radiusInTiles),
                       center + new Vector2(-radiusInTiles, radiusInTiles), Color.green, 2.0f);
        Debug.DrawLine(center + new Vector2(-radiusInTiles, radiusInTiles),
                       center + new Vector2(-radiusInTiles, -radiusInTiles), Color.green, 2.0f);

        for (int x = -diameterInTiles; x <= diameterInTiles; x++)
        {
            for (int y = -diameterInTiles; y <= diameterInTiles; y++)
            {
                // Do x * radiusInTiles for slope
                Vector2 position = new Vector2(center.x + x * tileSize, center.y + y * tileSize);
                if (Vector2.Distance(center, position) <= radiusInTiles)
                {
                    Debug.DrawLine(center, position, Color.red, 2.0f); // Visualize the positions
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
        int diameterInTiles = Mathf.CeilToInt(radius * tileSize * 2);
        for (int x = -diameterInTiles; x <= diameterInTiles; x++)
        {
            for (int y = -diameterInTiles; y <= diameterInTiles; y++)
            {
                Vector2 position = new Vector2(center.x + x * tileSize, center.y + y * tileSize);
                if (fill)
                    placeTile(position);
                else
                    removeTile(position);
            }
        }
    }
}
