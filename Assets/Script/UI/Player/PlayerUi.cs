using UnityEngine;

public class PlayerUi : BaseUi
{
    [SerializeField] private Hover hover;
    [SerializeField] private Pointer pointer;
    [SerializeField] private SummaryUi summary;

    public Hover LevelHover => hover;
    public Pointer LevelPointer => pointer;
    public SummaryUi Summary => summary;

    void Start()
    {
        this.summary.Hide();
    }

    public void Init(Map map)
    {
        this.hover.Init(map);
        this.pointer.Init(map);
    }

    public void Reset()
    {
        this.hover.Clear();
        this.pointer.Clear();
        this.summary.Hide();
    }
}
