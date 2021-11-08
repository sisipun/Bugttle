using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundTiles : MonoBehaviour
{
    public TileBase FirstTile;
    public TileBase SecondTile;
    public int MapSize = 8;

    private Tilemap map;
    private Camera mainCamera;

    void Start()
    {
        map = GetComponent<Tilemap>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            randomizeMap();
            // Vector3 clickWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // Vector3Int clickCell = map.WorldToCell(new Vector3(clickWorld.x, clickWorld.y, 0));
            // Debug.Log(clickCell);
            // map.SetTile(clickCell, TileToSet);
        }
    }

    void randomizeMap() {
        for (int i = 0; i < MapSize; i++) {
            for (int j = 0; j < MapSize; j++) {
                map.SetTile(new Vector3Int(i, j, 0), Random.Range(0, 2) == 0 ? FirstTile : SecondTile);
            }
        }
    }
}
