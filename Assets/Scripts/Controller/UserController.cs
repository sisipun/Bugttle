using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : BaseController
{
    private Camera mainCamera;
    private Vector2Int previouseMouseCell;
    private Bug selected;

    public override void Init(BaseLevel level, UserInterface ui, BugSide side)
    {
        base.Init(level, ui, side);
        this.mainCamera = Camera.main;
        this.previouseMouseCell = ((Vector2Int)level.LevelMap.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        this.selected = null;
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
            ui.LevelPointer.Clear();
            ui.LevelPointer.SetPosition(mouseCell);
            previouseMouseCell = mouseCell;
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
            Dictionary<Vector2Int, Path> possibleMoves = selected.PossibleMoves(level.LevelMap);
            if (possibleMoves.ContainsKey(click))
            {
                level.Move(selected.Position, click, possibleMoves[click]);
            }
            SetSelected(null);
            return;
        }

        if (selected != null && clicked != null)
        {
            List<Vector2Int> possibleAttacks = selected.PossibleAttacks(level.LevelMap);
            if (possibleAttacks.Contains(click))
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
        selected = bug;
        if (selected != null)
        {
            ui.LevelHover.SetMovable(selected.PossibleMoves(level.LevelMap).Keys);
            ui.LevelHover.SetAttackable(selected.PossibleAttacks(level.LevelMap));
        }
    }
}
