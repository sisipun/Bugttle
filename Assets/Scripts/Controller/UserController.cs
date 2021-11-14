using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : BaseController
{

    public override void handleInput()
    {
        // Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // Vector2Int mouseCell = ((Vector2Int)background.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        // if (mouseCell.x < 0 || mouseCell.y < 0 || mouseCell.x >= map.Size || mouseCell.y >= map.Size)
        // {
        //     return;
        // }

        // if (previouseMouseCell != mouseCell)
        // {
        //     pointer.Clear();
        //     pointer.SetPosition(mouseCell);
        //     previouseMouseCell = mouseCell;
        // }

        // if (Input.GetMouseButtonDown(0))
        // {
        //     onClick(mouseCell);
        // }
        // else if (Input.GetMouseButtonDown(1))
        // {
        //     hover.Clear();
        //     selected = null;
        // }
    }

    // private void onClick(Vector2Int click)
    // {
    //     Bug clicked = map.GetBug(click);

    //     if (selected == null && clicked != null)
    //     {
    //         SetSelected(clicked.IsUserSide ? clicked : null);
    //         return;
    //     }

    //     if (selected != null && clicked == null)
    //     {
    //         Dictionary<Vector2Int, Path> possibleMoves = selected.PossibleMoves(map);
    //         if (possibleMoves.ContainsKey(click))
    //         {
    //             Move(click, possibleMoves[click]);
    //         }
    //         SetSelected(null);
    //         return;
    //     }

    //     if (selected != null && clicked != null)
    //     {
    //         List<Vector2Int> possibleAttacks = selected.PossibleAttacks(map);
    //         if (possibleAttacks.Contains(click))
    //         {
    //             Attack(clicked);
    //             SetSelected(null);
    //         }
    //         else
    //         {
    //             SetSelected(clicked.IsUserSide ? clicked : null);
    //         }
    //         return;
    //     }
    // }
}
