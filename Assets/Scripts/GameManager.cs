using System.Collections;
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
    [SerializeField] private UserInterface ui;

    private BugSide currentSide;
    private Dictionary<BugSide, BaseController> controllers;
    private IEnumerator handleInput;

    public UserInterface GameUi => ui;
    public Map GameMap => map;
    public Background GameBackground => background;
    public Hover GameHover => hover;
    public Pointer GamePointer => pointer;
    public bool IsGameOver
    {
        get
        {
            bool hasGreen = false;
            bool hasRed = false;
            for (int x = 0; x < map.Size; x++)
            {
                for (int y = 0; y < map.Size; y++)
                {
                    Bug bug = map.GetBug(x, y);
                    if (bug != null)
                    {
                        if (bug.Side == BugSide.GREEN)
                        {
                            hasGreen = true;
                        }
                        if (bug.Side == BugSide.RED)
                        {
                            hasRed = true;
                        }
                    }
                }
            }

            return !hasGreen || !hasRed;
        }
    }

    void Start()
    {
        Init();
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
            map.RemoveBug(target.Position);
            if (IsGameOver)
            {
                Reset();
            }
        }
    }

    public void EndTurn()
    {
        StopCoroutine(handleInput);
        controllers[currentSide].EndTurn();
        currentSide = currentSide == BugSide.GREEN ? BugSide.RED : BugSide.GREEN;

        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                Bug bug = map.GetBug(x, y);
                if (bug != null && bug.Side == currentSide)
                {
                    bug.StartTurn();
                }
            }
        }
        controllers[currentSide].StartTurn();
        this.handleInput = controllers[currentSide].HandleInput();
        StartCoroutine(handleInput);
    }

    public void Reset()
    {
        this.map.Clear();
        this.background.Clear();
        this.hover.Clear();
        this.pointer.Clear();
        this.greenController.EndTurn();
        this.redController.EndTurn();
        Init();
    }

    private void Init()
    {
        this.map.Init();
        this.background.Init(map);
        this.hover.Init(map);
        this.pointer.Init(map);
        this.greenController.Init(this, BugSide.GREEN);
        this.redController.Init(this, BugSide.RED);

        this.bugGenerator.Init(map, background);
        this.bugGenerator.Generate();

        this.currentSide = BugSide.GREEN;
        this.controllers = new Dictionary<BugSide, BaseController>();
        this.controllers.Add(BugSide.GREEN, greenController);
        this.controllers.Add(BugSide.RED, redController);
        this.controllers[currentSide].StartTurn();
        this.handleInput = this.controllers[currentSide].HandleInput();
        StartCoroutine(this.handleInput);
    }
}