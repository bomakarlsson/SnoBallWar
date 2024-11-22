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

    Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
                                                          Input.mousePosition.y,
                                                          Camera.main.transform.position.z * -1));     
    }

    void placeTile(Vector3Int cellPosition)
    {
        tilemap.SetTile(cellPosition, tile);
    }

    void removeTile(Vector3Int cellPosition)
    {
        tilemap.SetTile(cellPosition, null);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            placeTile(tilemap.WorldToCell(GetMousePosition()));
        }
        else if (Input.GetMouseButtonDown(1))
        {
            removeTile(tilemap.WorldToCell(GetMousePosition()));
        }
    }
}
