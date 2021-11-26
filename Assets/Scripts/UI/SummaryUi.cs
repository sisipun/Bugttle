using UnityEngine;
using UnityEngine.UI;

public class SummaryUi : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text attackRangeText;
    [SerializeField] private Text moveRangeText;
    [SerializeField] private Text healthText;


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(Bug bug)
    {
        gameObject.SetActive(true);
        image.sprite = bug.Sprite;
        attackRangeText.text = bug.AttackRange.ToString();
        moveRangeText.text = bug.MoveRange.ToString();
        healthText.text = bug.Health.ToString();
    }
}
