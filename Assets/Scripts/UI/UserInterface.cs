using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private Hover hover;
    [SerializeField] private Pointer pointer;
    [SerializeField] private Button endTurnButton;

    public Hover LevelHover => hover;
    public Pointer LevelPointer => pointer;

    public void Init(Map map)
    {
        this.hover.Init(map);
        this.pointer.Init(map);
    }

    public void HideUi()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowUi()
    {
        this.gameObject.SetActive(true);
    }

    public void ClickEndTurnButton()
    {
        endTurnButton.onClick.Invoke();
    }

    public void Reset()
    {
        this.hover.Clear();
        this.pointer.Clear();
        this.ShowUi();
    }
}
