using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    [SerializeField] private LevelUi levelUi;
    [SerializeField] private Camera mainCamera;

    private Vector2Int previouseMouseCell;
    private Bug selected;
    private SkillType selectedSkillType;
    private List<Vector2Int> selectedZone;
    private List<Vector2Int> selectedTargets;

    public override void Init(BaseLevel level, BugSide side)
    {
        base.Init(level, side);
        this.previouseMouseCell = ((Vector2Int)level.Map.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        this.selectedZone = new List<Vector2Int>();
        this.selectedTargets = new List<Vector2Int>();
        Clear();
    }

    public override void OnStartTurn()
    {
        levelUi.Enable();
        base.OnStartTurn();
    }

    public override IEnumerator TurnAction()
    {
        while (true)
        {
            HandleInput();
            yield return null;
        }
    }

    public override void OnEndTurn()
    {
        base.OnEndTurn();
        levelUi.Disable();
        Clear();
    }

    public override void Clear()
    {
        this.levelUi.Clear();
        SetSelected(null);
    }

    public void EndTurn()
    {
        level.EndTurn();
    }

    public void SetSelected(Bug bug)
    {
        levelUi.Summary.Hide();
        selected?.SetOutlined(false);

        selected = bug;
        SetSelectedSkill(SkillType.MOVE);

        if (selected != null)
        {
            selected.SetOutlined(true);
            levelUi.Summary.Show(selected, this);
        }
    }

    public void SetSelectedSkill(SkillType skillType)
    {
        levelUi.Hover.Clear();
        levelUi.Pointer.Clear();
        selectedZone.Clear();
        selectedTargets.Clear();
        selectedSkillType = skillType;

        if (selected != null && selected.Side == Side)
        {
            BugSkill skill = selected.Skills[selectedSkillType];
            selectedZone = skill.GetZone(selected, level);
            selectedTargets = skill.GetTargets(selected, level);
            levelUi.Hover.Set(selectedZone, skill.ZoneTile);
            levelUi.Hover.Set(selectedTargets, skill.TargetTile);
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetSelected(null);
        }

        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mouseCell = ((Vector2Int)level.Map.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        if (mouseCell.x < 0 || mouseCell.y < 0 || mouseCell.x >= level.Map.Width || mouseCell.y >= level.Map.Height)
        {
            return;
        }

        if (previouseMouseCell != mouseCell)
        {
            onMouseCellChange(mouseCell);
        }

        if (Input.GetMouseButtonDown(0))
        {
            onClick(mouseCell);
        }
    }

    private void onMouseCellChange(Vector2Int newMouseCell)
    {
        levelUi.Pointer.Clear();
        levelUi.Pointer.SetPosition(newMouseCell);
        previouseMouseCell = newMouseCell;
        if (
            selected != null &&
            level.CurrentState == LevelState.TURN &&
            selectedSkillType == SkillType.MOVE &&
            selectedTargets.Contains(newMouseCell)
        )
        {
            Path path = level.Map.FindPath(selected.Position, newMouseCell);
            levelUi.Pointer.SetPath(path.Value);
        }
    }

    private void onClick(Vector2Int click)
    {
        Bug clicked = level.Map.GetBug(click);
        if (selectedTargets.Contains(click))
        {
            selected.Skills[selectedSkillType].Apply(selected, click, level);
            SetSelected(null);
        }
        else
        {
            SetSelected(clicked);
        }
    }
}
