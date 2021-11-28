using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryUi : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private Button[] buttons;

    public void Show(Bug bug)
    {
        gameObject.SetActive(true);
        image.sprite = bug.Sprite;

        if (playerController.Side != bug.Side)
        {
            return;
        }

        List<SkillType> skills = new List<SkillType>(bug.Skills.Keys);
        for (int i = 0; i < skills.Count && i < buttons.Length; i++)
        {
            Button button = buttons[i];
            BugSkill skill = bug.Skills[skills[i]];
            button.image.sprite = skill.Icon;
            button.onClick.AddListener(() => playerController.SetSelectedSkill(skill.Type()));
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
