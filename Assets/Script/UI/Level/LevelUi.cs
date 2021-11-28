using UnityEngine;
using UnityEngine.UI;

public class LevelUi : BaseUi
{
    [SerializeField] private Text levelStateTitle;

    public void SetLevelState(string state)
    {
        levelStateTitle.text = state;
    }
}
