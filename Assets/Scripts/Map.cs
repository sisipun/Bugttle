using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField]
    private Tilemap background;
    [SerializeField]
    private TileBase[] backgroundTiles;

    [SerializeField]
    private Tilemap hover;
    [SerializeField]
    private TileBase hoverTile;

    [SerializeField]
    private BugData[] bugsData;
    
    [SerializeField]
    private int mapSize = 8;

    private GameObject[,] map;
    private Grid grid;
    private Camera camera;

    private Vector3Int previouseMouseCell = new Vector3Int();
    
    private Vector3Int? selectedCell = null;


    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
        camera = Camera.main;
        map = new GameObject[mapSize, mapSize];

        RandomizeBackground();
        RandomizeBugs();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorld = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCell = grid.WorldToCell(new Vector2(mouseWorld.x, mouseWorld.y));
        if (previouseMouseCell != mouseCell && mouseCell.x >= 0 && mouseCell.y >= 0 && mouseCell.x < mapSize && mouseCell.y < mapSize) {
            hover.SetTile(previouseMouseCell, null);
            hover.SetTile(mouseCell, hoverTile);
            previouseMouseCell = mouseCell;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (selectedCell.HasValue && selectedCell != mouseCell && map[mouseCell.x, mouseCell.y] == null) {                
                GameObject selected = map[selectedCell.Value.x, selectedCell.Value.y];
                selected.transform.position = grid.CellToWorld(mouseCell);
                map[mouseCell.x, mouseCell.y] = selected;
                map[selectedCell.Value.x, selectedCell.Value.y] = null;
                selectedCell = null;
            } else if (map[mouseCell.x, mouseCell.y] != null) {
                selectedCell = mouseCell;
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            selectedCell = null;
        }
    }

    private void RandomizeBackground() {
        for (int i = 0; i < mapSize; i++) {
            for (int j = 0; j < mapSize; j++) {
                background.SetTile(new Vector3Int(i, j, 0), backgroundTiles[Random.Range(0, backgroundTiles.Length)]);
            }
        }
    }

    private void RandomizeBugs() {
        for (int i = 0; i < mapSize; i++) {
            int cell = Random.Range(0, mapSize);
            BugData bugData = bugsData[Random.Range(0, bugsData.Length)];
            Vector3 position = grid.CellToWorld(new Vector3Int(i, cell, 0));
            GameObject bug = Instantiate<GameObject>(bugData.Prefub, position, Quaternion.identity);
            Bug bugComponent = bug.GetComponent<Bug>();
            bugComponent.Init(bugData);
            map[i, cell] = bug;
        }
    }
}