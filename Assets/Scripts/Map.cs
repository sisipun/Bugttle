using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField]
    private Tilemap background;
    [SerializeField]
    private CellData[] backgroundCells;

    [SerializeField]
    private Tilemap hover;
    [SerializeField]
    private TileBase moveHoverTile;
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

    private Cell[,] map;
    private Vector2Int previouseMouseCell;
    private Bug selected;


    void Start()
    {
        grid = GetComponent<Grid>();
        mainCamera = Camera.main;

        map = new Cell[mapSize, mapSize];
        previouseMouseCell = ((Vector2Int)grid.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        selected = null;

        RandomizeBackground();
        RandomizeBugs();
    }

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
    public void EndTurn()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                Bug bug = map[x, y].Bug;
                if (bug != null && bug.IsUserSide)
                {
                    bug.EndTurn();
                }
            }
        }
    }

    private void onClick(Vector2Int click)
    {
        Bug clicked = map[click.x, click.y].Bug;

        if (selected == null && clicked != null)
        {
            SetSelected(clicked.IsUserSide ? clicked : null);
            return;
        }

        if (selected != null && clicked == null)
        {
            Dictionary<Vector2Int, Path> possibleMoves = selected.PossibleMoves(map);
            if (possibleMoves.ContainsKey(click))
            {
                Move(click, possibleMoves[click]);
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
                SetSelected(clicked.IsUserSide ? clicked : null);
            }
            return;
        }
    }

    private void Move(Vector2Int position, Path path)
    {
        selected.transform.position = grid.CellToWorld(((Vector3Int)position));
        map[selected.Position.x, selected.Position.y].Bug = null;
        map[position.x, position.y].Bug = selected;
        selected.Move(position, path);
    }

    private void Attack(Bug bug)
    {
        selected.Attack();
        bug.Damage();
        if (bug.IsDead)
        {
            map[bug.Position.x, bug.Position.y].Bug = null;
            Destroy(bug.gameObject);
        }
    }

    private void SetSelected(Bug bug)
    {
        ClearHover();
        selected = bug;
        if (selected != null)
        {
            ShowHover(selected.PossibleMoves(map).Keys, moveHoverTile);
            ShowHover(selected.PossibleAttacks(map), attackHoverTile);
        }
    }

    private void RandomizeBackground()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                Cell cell = new Cell(background, new Vector2Int(x, y), backgroundCells[Random.Range(0, backgroundCells.Length)]);
                map[x, y] = cell;
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
            bug.Init(position, x < mapSize / 2 ? Bug.Side.USER : Bug.Side.AI, bugData);
            map[x, y].Bug = bug;
        }
    }

    private void ShowHover(ICollection<Vector2Int> positions, TileBase tile)
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