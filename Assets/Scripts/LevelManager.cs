using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Map map;
    [SerializeField] private List<BaseLevel> levels;
    [SerializeField] private LevelUi ui;

    [SerializeField] private BugGenerator bugGenerator;
    [SerializeField] private BaseController greenController;
    [SerializeField] private BaseController redController;

    private Dictionary<BugSide, BaseController> sideToController;
    private Dictionary<LevelType, BaseLevel> typeToLevel;
    private IEnumerator handleInput;
    private BaseLevel level;

    void Awake()
    {
        this.sideToController = new Dictionary<BugSide, BaseController>();
        this.sideToController.Add(BugSide.GREEN, greenController);
        this.sideToController.Add(BugSide.RED, redController);

        this.typeToLevel = new Dictionary<LevelType, BaseLevel>();
        foreach(BaseLevel level in levels)
        {
            this.typeToLevel.Add(level.Type(), level);
        }
    }

    void Update()
    {
        if (level != null && level.IsGameOver())
        {
            RestartLevel();
        }
    }

    public void StartLevel(LevelType type)
    {
        level = typeToLevel[type];
        Init();
    }

    public void EndTurn()
    {
        StopCoroutine(handleInput);
        BaseController controller = sideToController[level.CurrentSide];
        sideToController[level.CurrentSide].EndTurn();

        this.level.EndTurn();

        sideToController[level.CurrentSide].StartTurn();
        this.handleInput = sideToController[level.CurrentSide].HandleInput();
        StartCoroutine(handleInput);
    }

    
    public void RestartLevel()
    {
        Reset();
        Init();
    }

    public void EndLevel()
    {
        Reset();
        level = null;
    }

    private void Reset()
    {
        this.map.Clear();
        this.level.Reset();
        this.ui.Reset();

        this.greenController.EndTurn();
        this.redController.EndTurn();
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

        BaseController controller = this.sideToController[level.CurrentSide];
        controller.StartTurn();
        this.handleInput = controller.HandleInput();
        StartCoroutine(this.handleInput);
    }
}