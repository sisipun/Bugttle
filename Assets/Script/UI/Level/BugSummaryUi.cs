using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BugSummaryUi : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button[] buttons;

    public void Show(Bug bug, PlayerController controller)
    {
        gameObject.SetActive(true);
        icon.sprite = bug.Sprite;

        if (controller.Side != bug.Side)
        {
            return;
        }

        List<SkillType> skills = new List<SkillType>(bug.Skills.Keys);
        for (int i = 0; i < skills.Count && i < buttons.Length; i++)
        {
            Button button = buttons[i];
            BugSkill skill = bug.Skills[skills[i]];
            button.image.sprite = skill.Icon;
            button.onClick.AddListener(() => controller.SetSelectedBugSkill(skill.Type()));
            button.gameObject.SetActive(true);
        }
    }
    
    public void Hide()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
        gameObject.SetActive(false);
    }
}
