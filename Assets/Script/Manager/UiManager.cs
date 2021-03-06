using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private MenuUi menuUi;
    [SerializeField] private LevelUi levelUi;
    [SerializeField] private GameOverUi gameOverUi;

    void Start()
    {
        menuUi.Show();
        levelUi.Hide();
        gameOverUi.Hide();
    }

    public void ShowLevel(BaseLevel level)
    {
        this.levelUi.Init(level.Map);

        menuUi.Hide();
        levelUi.Show();
        gameOverUi.Hide();
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
