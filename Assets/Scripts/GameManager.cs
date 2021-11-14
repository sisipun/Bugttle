using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Map map;
    [SerializeField] private Background background;
    [SerializeField] private Hover hover;
    [SerializeField] private Pointer pointer;
    [SerializeField] private BugGenerator bugGenerator;
    [SerializeField] private BaseController greenController;
    [SerializeField] private BaseController redController;

    private BugSide currentSide;

    public Map GameMap => map;
    public Background GameBackground => background;
    public Hover GameHover => hover;
    public Pointer GamePointer => pointer;

    void Start()
    {
        this.currentSide = BugSide.GREEN;

        map.Init();
        background.Init(map);
        hover.Init(map);
        pointer.Init(map);
        bugGenerator.Init(map, background);
        bugGenerator.Generate();
        greenController.Init(this, BugSide.GREEN);
        redController.Init(this, BugSide.RED);
    }

    void Update()
    {
        if (currentSide == BugSide.GREEN)
        {
            greenController.handleInput();
        }
        else if (currentSide == BugSide.RED)
        {
            redController.handleInput();
        }
    }

    public void Move(Vector2Int from, Vector2Int to, Path path)
    {
        Bug bug = map.GetBug(from);
        bug.transform.position = background.CellToWorld(to);
        map.SetBug(from, null);
        map.SetBug(to, bug);
        bug.Move(to, path);
    }

    public void Attack(Bug source, Bug target)
    {
        source.Attack();
        target.Damage();
        if (target.IsDead)
        {
            map.SetBug(target.Position, null);
            Destroy(target.gameObject);
        }
    }

    public void EndTurn()
    {
        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                Bug bug = map.GetBug(x, y);
                if (bug != null && bug.Side == currentSide)
                {
                    bug.EndTurn();
                }
            }
        }

        currentSide = currentSide == BugSide.GREEN ? BugSide.RED : BugSide.GREEN;
    }
}