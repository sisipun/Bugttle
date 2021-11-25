using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : BaseUi
{
    [SerializeField] private Hover hover;
    [SerializeField] private Pointer pointer;
    [SerializeField] private BugInfo bugInfo;

    public Hover LevelHover => hover;
    public Pointer LevelPointer => pointer;
    public BugInfo BugInfo => bugInfo;

    public void Init(Map map)
    {
        this.hover.Init(map);
        this.pointer.Init(map);
    }

    public void Reset()
    {
        this.hover.Clear();
        this.pointer.Clear();
        this.bugInfo.Hide();
    }
}
