using UnityEngine;
using UnityEngine.UI;

public class GameOverUi : BaseUi
{
    [SerializeField] private Text title;

    public void SetWinner(BugSide winner)
    {
        title.text = string.Format("Game Over.\nPlayer {0} won!", winner.ToString().ToLower());
    }
}
