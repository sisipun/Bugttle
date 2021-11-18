using UnityEngine;
using UnityEngine.UI;

public class LevelUi : MonoBehaviour
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

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
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
        this.Show();
    }
}
