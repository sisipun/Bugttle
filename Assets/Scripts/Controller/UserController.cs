using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : BaseController
{
    private Camera mainCamera;
    private Vector2Int previouseMouseCell;
    private Bug selected;
    private Dictionary<Vector2Int, Path> selectedPossibleMoves;
    private List<Vector2Int> selectedPossibleAttacks;

    public override void Init(BaseLevel level, UserInterface ui, BugSide side)
    {
        base.Init(level, ui, side);
        this.mainCamera = Camera.main;
        this.previouseMouseCell = ((Vector2Int)level.LevelMap.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        this.selected = null;
        this.selectedPossibleMoves = new Dictionary<Vector2Int, Path>();
        this.selectedPossibleAttacks = new List<Vector2Int>();
    }

    public override void StartTurn()
    {
        base.StartTurn();
        ui.ShowUi();
    }

    public override IEnumerator HandleInput()
    {
        while (true)
        {
            HandleInputIteration();
            yield return null;
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        ui.LevelHover.Clear();
        ui.LevelPointer.Clear();
        ui.HideUi();
        SetSelected(null);
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
                level.Move(selected.Position, click, selectedPossibleMoves[click]);
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
            foreach (KeyValuePair<Vector2Int, Path> move in selected.PossibleMoves(level.LevelMap))
            {
                selectedPossibleMoves.Add(move.Key, move.Value);
            }
            selectedPossibleAttacks.AddRange(selected.PossibleAttacks(level.LevelMap));
            ui.LevelHover.SetMovable(selectedPossibleMoves.Keys);
            ui.LevelHover.SetAttackable(selectedPossibleAttacks);
        }
    }
}
