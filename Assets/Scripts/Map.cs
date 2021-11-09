using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField]
    private Tilemap Background;
    [SerializeField]
    private TileBase[] BackgroundTiles;

    [SerializeField]
    private Tilemap Hover;
    [SerializeField]
    private TileBase HoverTile;

    [SerializeField]
    private GameObject BugPrefub;
    
    [SerializeField]
    private int MapSize = 8;

    private GameObject[,] map;
    private Grid grid;
    private Camera camera;

    private Vector3Int previouseMouseCell = new Vector3Int();
    
    private GameObject selected = null;
    private Vector3Int selectedCell;


    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
        camera = Camera.main;
        randomizeBackground();

        map = new GameObject[MapSize, MapSize];
        map[0, 0] = GameObject.Instantiate(BugPrefub, grid.CellToWorld(new Vector3Int(0, 0, 0)), Quaternion.identity);
        map[3, 6] = GameObject.Instantiate(BugPrefub, grid.CellToWorld(new Vector3Int(3, 6, 0)), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorld = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCell = grid.WorldToCell(new Vector2(mouseWorld.x, mouseWorld.y));
        if (previouseMouseCell != mouseCell) {
            Hover.SetTile(previouseMouseCell, null);
            Hover.SetTile(mouseCell, HoverTile);
            previouseMouseCell = mouseCell;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (selected == null) {
                selectedCell = mouseCell;
                selected = map[selectedCell.x, selectedCell.y];
            } else {
                selected.transform.position = grid.CellToWorld(mouseCell);
                map[mouseCell.x, mouseCell.y] = selected;
                map[selectedCell.x, selectedCell.y] = null;
                selected = null;
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            selected = null;
        }
    }

    void randomizeBackground() {
        for (int i = 0; i < MapSize; i++) {
            for (int j = 0; j < MapSize; j++) {
                Background.SetTile(new Vector3Int(i, j, 0), BackgroundTiles[Random.Range(0, BackgroundTiles.GetLength(0) - 1)]);
            }
        }
    }
}
