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

    private Grid grid;
    private Camera mainCamera;

    private Bug[,] map;
    private Vector2Int previouseMouseCell;
    private Bug selected;


    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
        mainCamera = Camera.main;

        map = new Bug[mapSize, mapSize];
        previouseMouseCell = ((Vector2Int)grid.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        selected = null;

        RandomizeBackground();
        RandomizeBugs();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mouseCell = ((Vector2Int)grid.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
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
            onClick(mouseCell);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ClearHover();
            selected = null;
        }
    }

    private void onClick(Vector2Int click)
    {
        Bug clicked = map[click.x, click.y];

        if (selected == null && clicked != null)
        {
            SetSelected(clicked);
            return;
        }

        if (selected != null && clicked == null)
        {
            List<Vector2Int> possibleTurns = selected.PossibleTurns(map);
            if (possibleTurns.Contains(click))
            {
                Turn(click);
            }
            SetSelected(null);
            return;
        }

        if (selected != null && clicked != null)
        {
            List<Vector2Int> possibleAttacks = selected.PossibleAttacks(map);
            if (possibleAttacks.Contains(click))
            {
                Attack(clicked);
                SetSelected(null);
            }
            else
            {
                SetSelected(clicked);
            }
            return;
        }
    }

    private void Turn(Vector2Int position)
    {
        selected.transform.position = grid.CellToWorld(((Vector3Int)position));
        map[selected.Position.x, selected.Position.y] = null;
        map[position.x, position.y] = selected;
        selected.Position = position;
    }

    private void Attack(Bug bug)
    {
        bug.Hit();
        if (bug.IsDead)
        {
            map[bug.Position.x, bug.Position.y] = null;
            Destroy(bug.gameObject);
        }
    }

    private void SetSelected(Bug bug)
    {
        ClearHover();
        selected = bug;
        if (selected != null)
        {
            ShowHover(selected.PossibleTurns(map), turnHoverTile);
            ShowHover(selected.PossibleAttacks(map), attackHoverTile);
        }
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

    private void ShowHover(List<Vector2Int> positions, TileBase tile)
    {
        foreach (Vector2Int position in positions)
        {
            hover.SetTile(((Vector3Int)position), tile);
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