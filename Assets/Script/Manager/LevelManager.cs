using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ControllerManager controllerManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private Map map;
    [SerializeField] private BaseLevel[] levels;
    [SerializeField] private BugGenerator bugGenerator;
    [SerializeField] private CellGenerator cellGenerator;
    [SerializeField] private int minWidth;
    [SerializeField] private int maxWidth;
    [SerializeField] private int minHeight;
    [SerializeField] private int maxHeight;


    private Dictionary<LevelType, BaseLevel> typeToLevel;
    private BaseLevel level;
    private LevelData levelData;

    void Awake()
    {
        this.typeToLevel = new Dictionary<LevelType, BaseLevel>();
        foreach (BaseLevel level in levels)
        {
            level.OnEndTurn += OnEndTurn;
            level.OnGameOver += OnGameOver;
            this.typeToLevel.Add(level.Type(), level);
        }
    }

    public void StartLevel(LevelData data)
    {
        this.map.Init(Random.Range(minWidth, maxWidth + 1), Random.Range(minHeight, maxHeight + 1));

        this.cellGenerator.Generate(map);
        this.bugGenerator.Generate(map, 3);

        this.levelData = data;
        this.level = typeToLevel[levelData.Type];
        this.level.Init(map);

        uiManager.ShowLevel(level);
        uiManager.UpdateLevelState(level);
        controllerManager.StartLevel(level);
    }

    public void RestartLevel()
    {
        Reset();
        StartLevel(levelData);
    }

    public void ExitToMenu()
    {
        Reset();
        uiManager.ShowMenu();
    }

    private void OnGameOver(BugSide winner)
    {
        controllerManager.EndLevel();
        uiManager.ShowGameOver(winner);
    }

    private void OnEndTurn(BugSide started)
    {
        controllerManager.EndTurn();
        uiManager.UpdateLevelState(level);
        controllerManager.StartTurn(started);
    }

    private void Reset()
    {
        controllerManager.EndLevel();
        this.map.Clear();
    }
}