using UnityEngine;
using UnityEngine.UI;

public class GameOverUi : BaseUi
{
    [SerializeField] private Text title;

    public void SetWinner(BugSide winner)
    {
        title.text = string.Format("Game Over, {0} won!", winner);
    }
}
