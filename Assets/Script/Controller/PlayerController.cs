using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    [SerializeField] private LevelUi levelUi;
    [SerializeField] private Camera mainCamera;

    private Bug selectedBug;
    private Cell hoveredCell;
    private SkillType selectedBugSkillType;
    private List<Vector2Int> selectedBugZone;
    private List<Vector2Int> selectedBugTargets;

    public override void Init(BaseLevel level, BugSide side)
    {
        base.Init(level, side);
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
        SetHoveredCell(null);
        SetSelectedBug(null);
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
            SetSelectedBug(null);
        }

        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mouseCell = ((Vector2Int)level.Map.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
        if (mouseCell.x < 0 || mouseCell.y < 0 || mouseCell.x >= level.Map.Width || mouseCell.y >= level.Map.Height)
        {
            SetHoveredCell(null);
            return;
        }

        if (hoveredCell?.Position != mouseCell)
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
        SetHoveredCell(level.Map.GetCell(hover));

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
            SetSelectedBug(null);
        }
        else
        {
            SetSelectedBug(level.Map.GetCell(click)?.Bug);
        }
    }

    private void SetHoveredCell(Cell cell)
    {
        ClearUi();
        hoveredCell = cell;
        ShowUi();
    }

    private void SetSelectedBug(Bug bug)
    {
        ClearUi();
        selectedBug = bug;
        SetSelectedBugSkill(SkillType.MOVE);
        ShowUi();
    }

    private void ClearUi() {
        levelUi.CellSummary.Hide();
        levelUi.BugSummary.Hide();
        levelUi.Pointer.Clear();

        if (hoveredCell != null && hoveredCell.Bug != null)
        {
            hoveredCell.Bug.ShowHealthBar(false);
        }

        if (selectedBug != null)
        {
            selectedBug.ShowHealthBar(false);
            selectedBug.ShowOutline(false);
        }
    }

    private void ShowUi() {
        if (hoveredCell != null)
        {
            levelUi.CellSummary.Show(hoveredCell);
            levelUi.Pointer.SetPosition(hoveredCell.Position);
            if (hoveredCell.Bug != null)
            {
                hoveredCell.Bug.ShowHealthBar(true);
            }
        }

        if (selectedBug != null)
        {
            selectedBug.ShowHealthBar(true);
            selectedBug.ShowOutline(true);
        }

        Bug summary = selectedBug == null ? hoveredCell?.Bug : selectedBug;
        if (summary != null)
        {
            levelUi.BugSummary.Show(summary, this);
        } 
    }
}
