using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelUi levelUi;
    [SerializeField] private Map map;
    [SerializeField] private BaseLevel[] levels;

    [SerializeField] private BugGenerator bugGenerator;
    [SerializeField] private BaseController greenController;
    [SerializeField] private BaseController redController;

    private Dictionary<BugSide, BaseController> sideToController;
    private Dictionary<LevelType, BaseLevel> typeToLevel;
    private IEnumerator currentAction;
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
        if (currentAction != null && sideToController[level.CurrentSide].IsTurnEnded)
        {
            EndTurn();
        }
    }

    public void StartLevel(LevelType type)
    {
        level = typeToLevel[type];
        levelUi.Show();
        Init();
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

        this.greenController.EndTurn();
        this.redController.EndTurn();
        
        this.greenController.Reset();
        this.redController.Reset();
    }

    private void Init()
    {
        this.map.Init();
        this.level.Init(map);
        this.greenController.Init(level, BugSide.GREEN);
        this.redController.Init(level, BugSide.RED);

        this.bugGenerator.Init(map);
        this.bugGenerator.Generate(3);

        BaseController controller = this.sideToController[level.CurrentSide];
        controller.StartTurn();
        this.currentAction = controller.TurnAction();
        StartCoroutine(this.currentAction);
    }
}