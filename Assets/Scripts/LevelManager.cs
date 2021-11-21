using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField] private Map map;
    [SerializeField] private BaseLevel[] levels;

    [SerializeField] private BugGenerator bugGenerator;
    [SerializeField] private BaseController greenController;
    [SerializeField] private BaseController redController;

    private Dictionary<BugSide, BaseController> sideToController;
    private Dictionary<LevelType, BaseLevel> typeToLevel;
    private IEnumerator currentAction;
    private BaseLevel level;
    private LevelType type;

    void Awake()
    {
        this.sideToController = new Dictionary<BugSide, BaseController>();
        this.sideToController.Add(BugSide.GREEN, greenController);
        this.sideToController.Add(BugSide.RED, redController);

        this.typeToLevel = new Dictionary<LevelType, BaseLevel>();
        foreach (BaseLevel level in levels)
        {
            this.typeToLevel.Add(level.Type(), level);
        }
    }

    void Update()
    {
        if (level != null && level.GetWinner().HasValue)
        {
            GameOver(level.GetWinner().Value);
        }
        if (currentAction != null && sideToController[level.CurrentSide].IsTurnEnded)
        {
            EndTurn();
        }
    }

    public void StartLevel(LevelData data)
    {
        this.type = data.Type;
        Init();
        uiManager.ShowLevel();
    }

    public void RestartLevel()
    {
        Reset();
        Init();
        uiManager.ShowLevel();
    }

    public void GameOver(BugSide winner)
    {
        Reset();
        uiManager.ShowGameOver(winner);
    }

    public void ExitToMenu()
    {
        Reset();
        uiManager.ShowMenu();
    }

    public void EndTurn()
    {
        StopCoroutine(currentAction);
        BaseController controller = sideToController[level.CurrentSide];
        sideToController[level.CurrentSide].EndTurn();

        this.level.EndTurn();

        sideToController[level.CurrentSide].StartTurn();
        this.currentAction = sideToController[level.CurrentSide].TurnAction();
        StartCoroutine(currentAction);
    }

    private void Reset()
    {
        this.greenController.EndTurn();
        this.redController.EndTurn();

        if (currentAction != null)
        {
            StopCoroutine(currentAction);
            currentAction = null;
        }

        this.greenController.Reset();
        this.redController.Reset();

        if (level != null)
        {
            this.level.Reset();
        }
        this.map.Clear();
        level = null;
    }

    private void Init()
    {
        level = typeToLevel[type];

        this.map.Init();

        this.bugGenerator.Init(map);
        this.bugGenerator.Generate(3);

        this.level.Init(map);
        this.greenController.Init(level, BugSide.GREEN);
        this.redController.Init(level, BugSide.RED);

        BaseController controller = this.sideToController[level.CurrentSide];
        controller.StartTurn();
        this.currentAction = controller.TurnAction();
        StartCoroutine(this.currentAction);
    }
}