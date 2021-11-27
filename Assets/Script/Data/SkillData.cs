using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/New Skill")]
public class SkillData : ScriptableObject
{
    [SerializeField] private int range;
    [SerializeField] private int count;
    [SerializeField] private BaseSkillEffect skillEffect;

    public int Range => range;
    public int Count => count;
    public BaseSkillEffect SkillEffect => skillEffect;
}
