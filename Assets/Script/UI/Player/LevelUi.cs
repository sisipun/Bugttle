using UnityEngine;
using UnityEngine.UI;

public class LevelUi : BaseUi
{
    [SerializeField] private Hover hover;
    [SerializeField] private Pointer pointer;
    [SerializeField] private SummaryUi summary;
    [SerializeField] private Button endTurnButton;

    public Hover Hover => hover;
    public Pointer Pointer => pointer;
    public SummaryUi Summary => summary;

    public void Init(Map map)
    {
        Clear();
        this.hover.Init(map.Width, map.Height);
        this.pointer.Init(map.Width, map.Height);
    }

    public void Enable()
    {
        endTurnButton.interactable = true;
    }

    public void Disable()
    {
        endTurnButton.interactable = false;
    }
    
    public void Clear()
    {
        this.hover.Clear();
        this.pointer.Clear();
        this.summary.Hide();
    }
}
