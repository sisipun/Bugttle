using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Map map;
    [SerializeField] private BaseLevel level;
    [SerializeField] private UserInterface ui;

    [SerializeField] private BugGenerator bugGenerator;
    [SerializeField] private BaseController greenController;
    [SerializeField] private BaseController redController;

    private Dictionary<BugSide, BaseController> controllers;
    private IEnumerator handleInput;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (level.IsGameOver())
        {
            Reset();
        }
    }

    public void EndTurn()
    {
        StopCoroutine(handleInput);
        controllers[level.CurrentSide].EndTurn();

        this.level.EndTurn();

        controllers[level.CurrentSide].StartTurn();
        this.handleInput = controllers[level.CurrentSide].HandleInput();
        StartCoroutine(handleInput);
    }

    public void Reset()
    {
        this.map.Clear();
        this.level.Reset();
        this.ui.Reset();

        this.greenController.EndTurn();
        this.redController.EndTurn();

        Init();
    }

    private void Init()
    {
        this.map.Init();
        this.level.Init(map);
        this.ui.Init(map);
        this.greenController.Init(level, ui, BugSide.GREEN);
        this.redController.Init(level, ui, BugSide.RED);

        this.bugGenerator.Init(map);
        this.bugGenerator.Generate();

        this.controllers = new Dictionary<BugSide, BaseController>();
        this.controllers.Add(BugSide.GREEN, greenController);
        this.controllers.Add(BugSide.RED, redController);
        this.controllers[level.CurrentSide].StartTurn();
        this.handleInput = this.controllers[level.CurrentSide].HandleInput();
        StartCoroutine(this.handleInput);
    }
}