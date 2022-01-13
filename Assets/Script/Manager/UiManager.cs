using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private MenuUi menuUi;
    [SerializeField] private LevelUi levelUi;
    [SerializeField] private GameOverUi gameOverUi;
    [SerializeField] private PlayerUi playerUi;

    void Start()
    {
        menuUi.Show();
        levelUi.Hide();
        gameOverUi.Hide();
        playerUi.Hide();
    }

    public void ShowLevel(BaseLevel level)
    {
        menuUi.Hide();
        levelUi.Show();
        gameOverUi.Hide();
    }

    public void UpdateLevelState(BaseLevel level)
    {
        levelUi.SetLevelState(level.CurrentStateText());
    }

    public void ShowGameOver(BugSide winner)
    {
        menuUi.Hide();
        levelUi.Hide();
        gameOverUi.SetWinner(winner);
        gameOverUi.Show();
    }

    public void ShowMenu()
    {
        menuUi.Show();
        levelUi.Hide();
        gameOverUi.Hide();
    }
}
