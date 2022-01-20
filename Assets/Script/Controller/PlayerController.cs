using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    [SerializeField] private LevelUi levelUi;
    [SerializeField] private Camera mainCamera;

    private Vector2Int previouseMouseCell;
    private Bug selectedBug;
    private Bug hoveredBug;
    private SkillType selectedBugSkillType;
    private List<Vector2Int> selectedBugZone;
    private List<Vector2Int> selectedBugTargets;

    public override void Init(BaseLevel level, BugSide side)
    {
        base.Init(level, side);
        this.previouseMouseCell = ((Vector2Int)level.Map.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        this.selectedBugSkillType = SkillType.MOVE;
        this.selectedBugZone = new List<Vector2Int>();
        this.selectedBugTargets = new List<Vector2Int>();
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

    public void SetSelectedBugSkill(SkillType skillType)
    {
        levelUi.Hover.Clear();
        levelUi.Pointer.Clear();
        selectedBugZone.Clear();
        selectedBugTargets.Clear();
        selectedBugSkillType = skillType;

        if (selectedBug?.Side == Side)
        {
            BugSkill skill = selectedBug.Skills[selectedBugSkillType];
            selectedBugZone = skill.GetZone(selectedBug, level);
            selectedBugTargets = skill.GetTargets(selectedBug, level);
            levelUi.Hover.Set(selectedBugZone, skill.ZoneTile);
            levelUi.Hover.Set(selectedBugTargets, skill.TargetTile);
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
            setHovered(null);
            return;
        }

        if (previouseMouseCell != mouseCell)
        {
            onHover(mouseCell);
        }

        if (Input.GetMouseButtonDown(0))
        {
            onClick(mouseCell);
        }
    }

    private void onHover(Vector2Int hover)
    {
        levelUi.Pointer.Clear();
        levelUi.Pointer.SetPosition(hover);
        previouseMouseCell = hover;
        setHovered(level.Map.GetCell(hover));
        if (
            selectedBug != null &&
            level.CurrentState == LevelState.TURN &&
            selectedBugSkillType == SkillType.MOVE &&
            selectedBugTargets.Contains(hover)
        )
        {
            Path path = level.Map.FindPath(selectedBug.Position, hover);
            levelUi.Pointer.SetPath(path.Value);
        }
    }

    private void onClick(Vector2Int click)
    {
        if (selectedBugTargets.Contains(click))
        {
            selectedBug.Skills[selectedBugSkillType].Apply(selectedBug, click, level);
            SetSelected(null);
        }
        else
        {
            SetSelected(level.Map.GetCell(click));
        }
    }

    private void setHovered(Cell cell)
    {
        levelUi.CellSummary.Hide();

        if (cell != null)
        {
            levelUi.CellSummary.Show(cell);
        }

        if (hoveredBug != selectedBug)
        {
            hoveredBug?.ShowHealthBar(false);
        }

        hoveredBug = cell?.Bug;
        hoveredBug?.ShowHealthBar(true);

        if (selectedBug != null)
        {
            return;
        }

        levelUi.BugSummary.Hide();
        if (hoveredBug != null)
        {
            levelUi.BugSummary.Show(hoveredBug, this);
        }
    }

    private void SetSelected(Cell cell)
    {
        levelUi.CellSummary.Hide();
        if (cell != null)
        {
            levelUi.CellSummary.Show(cell);
        }

        levelUi.BugSummary.Hide();
        selectedBug?.ShowHealthBar(false);
        selectedBug?.SetOutlined(false);

        selectedBug = cell?.Bug;
        selectedBug?.SetOutlined(true);
        selectedBug?.ShowHealthBar(true);
        SetSelectedBugSkill(SkillType.MOVE);

        if (selectedBug != null)
        {
            levelUi.BugSummary.Show(selectedBug, this);
        }
    }
}
