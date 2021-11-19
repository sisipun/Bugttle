using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : BaseController
{
    private List<Bug> bugs;

    public override void Init(BaseLevel level, BugSide side)
    {
        base.Init(level, side);
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

    public override IEnumerator TurnAction()
    {
        foreach (Bug bug in bugs)
        {
            yield return StartCoroutine(MakeBugTurn(bug));
        }

        turnEnded = true;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        bugs.Clear();
    }

    private IEnumerator MakeBugTurn(Bug bug)
    {
        List<Vector2Int> attacks = level.GetPossibleAttacks(bug);
        if (attacks.Count > 0)
        {
            level.Attack(bug, level.LevelMap.GetBug(attacks[Random.Range(0, attacks.Count)]));
            yield return new WaitForSeconds(1.0f);
            yield break;
        }

        Dictionary<Vector2Int, Path> moves = level.GetPossibleMoves(bug);
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
            yield return new WaitForSeconds(1.0f);
        }

        attacks = level.GetPossibleAttacks(bug);
        if (attacks.Count > 0)
        {
            level.Attack(bug, level.LevelMap.GetBug(attacks[Random.Range(0, attacks.Count)]));
            yield return new WaitForSeconds(1.0f);
        }
    }
}
