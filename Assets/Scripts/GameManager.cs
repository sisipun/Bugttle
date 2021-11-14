using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Map map;
    [SerializeField] private Background background;
    [SerializeField] private Hover hover;
    [SerializeField] private Pointer pointer;
    [SerializeField] private BugGenerator bugGenerator;

    private Camera mainCamera;

    private Vector2Int previouseMouseCell;
    private Bug selected;

    void Awake()
    {
        this.mainCamera = Camera.main;
    }

    void Start()
    {
        this.previouseMouseCell = ((Vector2Int)background.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        this.selected = null;

        map.Init();
        background.Init(map);
        hover.Init(map);
        pointer.Init(map);
        bugGenerator.Init(map, background);
        bugGenerator.Generate();
    }

    void Update()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mouseCell = ((Vector2Int)background.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        if (mouseCell.x < 0 || mouseCell.y < 0 || mouseCell.x >= map.Size || mouseCell.y >= map.Size)
        {
            return;
        }

        if (previouseMouseCell != mouseCell)
        {
            pointer.Clear();
            pointer.SetPosition(mouseCell);
            previouseMouseCell = mouseCell;
        }

        if (Input.GetMouseButtonDown(0))
        {
            onClick(mouseCell);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            hover.Clear();
            selected = null;
        }
    }

    public void EndTurn()
    {
        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                Bug bug = map.GetBug(x, y);
                if (bug != null && bug.IsUserSide)
                {
                    bug.EndTurn();
                }
            }
        }
    }

    private void onClick(Vector2Int click)
    {
        Bug clicked = map.GetBug(click);

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
        selected.transform.position = background.CellToWorld(position);
        map.SetBug(selected.Position, null);
        map.SetBug(position, selected);
        selected.Move(position, path);
    }

    private void Attack(Bug bug)
    {
        selected.Attack();
        bug.Damage();
        if (bug.IsDead)
        {
            map.SetBug(bug.Position, null);
            Destroy(bug.gameObject);
        }
    }

    private void SetSelected(Bug bug)
    {
        hover.Clear();
        selected = bug;
        if (selected != null)
        {
            hover.SetMovable(selected.PossibleMoves(map).Keys);
            hover.SetAttackable(selected.PossibleAttacks(map));
        }
    }
}