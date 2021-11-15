using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserController", menuName = "Scriptable Objects/Controllers/User Controller")]
public class UserController : BaseController
{
    private Camera mainCamera;
    private Vector2Int previouseMouseCell;
    private Bug selected;

    public override void Init(GameManager game, BugSide side)
    {
        base.Init(game, side);
        this.mainCamera = Camera.main;
        this.previouseMouseCell = ((Vector2Int)game.GameBackground.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        this.selected = null;
    }

    public override void HandleInput()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mouseCell = ((Vector2Int)game.GameBackground.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        if (mouseCell.x < 0 || mouseCell.y < 0 || mouseCell.x >= game.GameMap.Size || mouseCell.y >= game.GameMap.Size)
        {
            return;
        }

        if (previouseMouseCell != mouseCell)
        {
            game.GamePointer.Clear();
            game.GamePointer.SetPosition(mouseCell);
            previouseMouseCell = mouseCell;
        }

        if (Input.GetMouseButtonDown(0))
        {
            onClick(mouseCell);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            game.GameHover.Clear();
            selected = null;
        }
    }

    public override void EndTurn()
    {
        game.GameHover.Clear();
        game.GamePointer.Clear();
    }

    private void onClick(Vector2Int click)
    {
        Bug clicked = game.GameMap.GetBug(click);

        if (selected == null && clicked != null)
        {
            SetSelected(clicked.Side == side ? clicked : null);
            return;
        }

        if (selected != null && clicked == null)
        {
            Dictionary<Vector2Int, Path> possibleMoves = selected.PossibleMoves(game.GameMap);
            if (possibleMoves.ContainsKey(click))
            {
                game.Move(selected.Position, click, possibleMoves[click]);
            }
            SetSelected(null);
            return;
        }

        if (selected != null && clicked != null)
        {
            List<Vector2Int> possibleAttacks = selected.PossibleAttacks(game.GameMap);
            if (possibleAttacks.Contains(click))
            {
                game.Attack(selected, clicked);
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
        game.GameHover.Clear();
        selected = bug;
        if (selected != null)
        {
            game.GameHover.SetMovable(selected.PossibleMoves(game.GameMap).Keys);
            game.GameHover.SetAttackable(selected.PossibleAttacks(game.GameMap));
        }
    }
}
