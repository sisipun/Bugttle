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
    private TileBase turnHoverTile;
    [SerializeField]
    private TileBase attackHoverTile;

    [SerializeField]
    private Tilemap pointer;
    [SerializeField]
    private TileBase pointerTile;


    [SerializeField]
    private BugData[] bugsData;

    [SerializeField]
    private int mapSize = 8;

    private Bug[,] map;
    private Grid grid;
    private Camera mainCamera;

    private Vector2Int previouseMouseCell = new Vector2Int();

    private Bug selected = null;


    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
        mainCamera = Camera.main;
        map = new Bug[mapSize, mapSize];

        RandomizeBackground();
        RandomizeBugs();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mouseCell = ((Vector2Int)grid.WorldToCell(mouseWorld));
        if (mouseCell.x < 0 || mouseCell.y < 0 || mouseCell.x >= mapSize || mouseCell.y >= mapSize)
        {
            return;
        }

        if (previouseMouseCell != mouseCell)
        {
            pointer.SetTile(((Vector3Int)previouseMouseCell), null);
            pointer.SetTile(((Vector3Int)mouseCell), pointerTile);
            previouseMouseCell = mouseCell;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ClearHover();
            onTurn(mouseCell);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ClearHover();
            selected = null;
        }
    }

    private void onTurn(Vector2Int turn)
    {
        Bug clicked = map[turn.x, turn.y];
        if (clicked != null)
        {
            selected = clicked;
            ShowPossibleTurns(selected.PossibleTurns(map));
            return;
        }
        if (selected == null)
        {
            return;
        }

        List<Vector2Int> possibleTurns = selected.PossibleTurns(map);
        if (possibleTurns.Contains(turn))
        {
            selected.transform.position = grid.CellToWorld(((Vector3Int)turn));
            map[selected.Position.x, selected.Position.y] = null;
            map[turn.x, turn.y] = selected;
            selected.Position = turn;
        }

        selected = null;
    }

    private void RandomizeBackground()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                background.SetTile(((Vector3Int)new Vector2Int(x, y)), backgroundTiles[Random.Range(0, backgroundTiles.Length)]);
            }
        }
    }

    private void RandomizeBugs()
    {
        for (int x = 0; x < mapSize; x++)
        {
            int y = Random.Range(0, mapSize);
            BugData bugData = bugsData[Random.Range(0, bugsData.Length)];
            Vector2Int position = new Vector2Int(x, y);
            Bug bug = Instantiate<GameObject>(
                bugData.Prefub,
                grid.CellToWorld(((Vector3Int)new Vector2Int(position.x, position.y))),
                 Quaternion.identity
            ).GetComponent<Bug>();
            bug.Init(position, bugData);
            map[x, y] = bug;
        }
    }

    private void ShowPossibleTurns(List<Vector2Int> turns)
    {
        foreach (Vector2Int turn in turns)
        {
            hover.SetTile(((Vector3Int)turn), turnHoverTile);
        }
    }

    private void ClearHover()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                hover.SetTile(((Vector3Int)new Vector2Int(x, y)), null);
            }
        }
    }
}