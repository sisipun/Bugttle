using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUi : BaseUi
{
    [SerializeField] private LevelManager levelManager;

    public void StartKillAllLevel()
    {
        Hide();
        levelManager.StartLevel(LevelType.KILL_ALL);
    }

    public void StartDoNotDieLevel()
    {
        Hide();
        levelManager.StartLevel(LevelType.DO_NOT_DIE);
    }
}
