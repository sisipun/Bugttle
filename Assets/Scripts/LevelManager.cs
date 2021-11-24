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
        this.map.Init();

        this.bugGenerator.Init(map);
        this.bugGenerator.Generate(3);

        this.levelData = data;
        this.level = typeToLevel[levelData.Type];
        this.level.Init(map);

        uiManager.ShowLevel();
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
        controllerManager.StartTurn(started);
    }

    private void Reset()
    {
        controllerManager.EndLevel();
        this.level.Reset();
        this.map.Clear();
    }
}