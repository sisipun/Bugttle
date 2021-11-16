using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AiController", menuName = "Scriptable Objects/Controllers/Ai Controller")]
public class AiController : BaseController
{
    private List<Bug> bugs;

    public override void Init(BaseLevel level, UserInterface ui, BugSide side)
    {
        base.Init(level, ui, side);
        this.bugs = new List<Bug>();
    }

    public override void StartTurn()
    {
        base.StartTurn();
        Map map = level.LevelMap;
        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                Bug bug = map.GetBug(x, y);
                if (bug != null && bug.Side == side)
                {
                    bugs.Add(bug);
                }
            }
        }
    }

    public override IEnumerator HandleInput()
    {
        foreach (Bug bug in bugs)
        {
            MakeBugTurn(bug);
            yield return new WaitForSeconds(1.0f);
        }

        ui.ClickEndTurnButton();
    }

    public override void EndTurn()
    {
        base.EndTurn();
        bugs.Clear();
    }

    private void MakeBugTurn(Bug bug)
    {
        List<Vector2Int> attacks = bug.PossibleAttacks(level.LevelMap);
        if (attacks.Count > 0)
        {
            level.Attack(bug, level.LevelMap.GetBug(attacks[Random.Range(0, attacks.Count)]));
            return;
        }

        Dictionary<Vector2Int, Path> moves = bug.PossibleMoves(level.LevelMap);
        if (moves.Count > 0)
        {
            KeyValuePair<Vector2Int, Path> currentMove;
            int currentMoveNumber = Random.Range(0, moves.Count);
            int i = 0;
            foreach (KeyValuePair<Vector2Int, Path> move in moves)
            {
                if (i >= currentMoveNumber)
                {
                    currentMove = move;
                    break;
                }
                i++;
            }
            level.Move(bug.Position, currentMove.Key, currentMove.Value);
        }

        attacks = bug.PossibleAttacks(level.LevelMap);
        if (attacks.Count > 0)
        {
            level.Attack(bug, level.LevelMap.GetBug(attacks[Random.Range(0, attacks.Count)]));
        }
    }
}
