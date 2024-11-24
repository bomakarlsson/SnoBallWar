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
}
