using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : BaseController
{
    [SerializeField] private PlayerUi ui;

    private Camera mainCamera;
    private Vector2Int previouseMouseCell;
    private Bug selected;
    private Dictionary<Vector2Int, Path> selectedPossibleMoves;
    private List<Vector2Int> selectedPossibleAttacks;

    public override void Init(BaseLevel level, BugSide side)
    {
        base.Init(level, side);
        this.ui.Init(level.LevelMap);
        this.mainCamera = Camera.main;
        this.previouseMouseCell = ((Vector2Int)level.LevelMap.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        this.selected = null;
        this.selectedPossibleMoves = new Dictionary<Vector2Int, Path>();
        this.selectedPossibleAttacks = new List<Vector2Int>();
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();
        ui.Show();
    }

    public override IEnumerator TurnAction()
    {
        while (true)
        {
            HandleInputIteration();
            yield return null;
        }
    }

    public override void OnEndTurn()
    {
        base.OnEndTurn();
        ui.LevelHover.Clear();
        ui.LevelPointer.Clear();
        ui.Hide();
        SetSelected(null);
    }

    public override void Reset()
    {
        this.ui.Reset();
    }

    public void EndTurn()
    {
        level.EndTurn();
    }

    private void HandleInputIteration()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mouseCell = ((Vector2Int)level.LevelMap.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        if (mouseCell.x < 0 || mouseCell.y < 0 || mouseCell.x >= level.LevelMap.Size || mouseCell.y >= level.LevelMap.Size)
        {
            return;
        }

        if (previouseMouseCell != mouseCell)
        {
            onMouseCellChange(mouseCell);
        }

        if (Input.GetMouseButtonDown(0))
        {
            onClick(mouseCell);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ui.LevelHover.Clear();
            selected = null;
        }
    }

    private void onMouseCellChange(Vector2Int newMouseCell)
    {
        ui.LevelPointer.Clear();
        ui.LevelPointer.SetPosition(newMouseCell);
        previouseMouseCell = newMouseCell;
        if (selected != null && selectedPossibleMoves.ContainsKey(newMouseCell))
        {
            Path path = selectedPossibleMoves[newMouseCell];
            ui.LevelPointer.SetPath(path.Value);
        }
    }

    private void onClick(Vector2Int click)
    {
        Bug clicked = level.LevelMap.GetBug(click);

        if (selected == null && clicked != null)
        {
            SetSelected(clicked.Side == side ? clicked : null);
            return;
        }

        if (selected != null && clicked == null)
        {
            if (selectedPossibleMoves.ContainsKey(click))
            {
                level.Move(selected, click, selectedPossibleMoves[click]);
            }
            SetSelected(null);
            return;
        }

        if (selected != null && clicked != null)
        {
            if (selectedPossibleAttacks.Contains(click))
            {
                level.Attack(selected, clicked);
                SetSelected(null);
            }
            else
            {
                SetSelected(clicked.Side == side ? clicked : null);
            }
            return;
        }
    }

    private void SetSelected(Bug bug)
    {
        ui.LevelHover.Clear();
        selectedPossibleMoves.Clear();
        selectedPossibleAttacks.Clear();
        if (selected != null)
        {
            selected.ResetOutlined();
        }

        selected = bug;
        if (selected != null)
        {
            selected.SetOutlined();
            foreach (KeyValuePair<Vector2Int, Path> move in level.GetPossibleMoves(selected))
            {
                selectedPossibleMoves.Add(move.Key, move.Value);
            }
            selectedPossibleAttacks.AddRange(level.GetPossibleAttacks(selected));
            ui.LevelHover.SetMovable(selectedPossibleMoves.Keys);
            ui.LevelHover.SetAttackable(selectedPossibleAttacks);
        }
    }
}
