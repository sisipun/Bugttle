using UnityEngine;

[CreateAssetMenu(fileName = "Bug", menuName = "Scriptable Objects/New Bug")]
public class BugData : ScriptableObject
{
    [SerializeField] private GameObject prefub;
    [SerializeField] private Sprite greenBody;
    [SerializeField] private Color greenColor;
    [SerializeField] private Sprite redBody;
    [SerializeField] private Color redColor;
    [SerializeField] private SkillData[] skills;
    [SerializeField] private int health;

    public GameObject Prefub => prefub;
    public Sprite GreenBody => greenBody;
    public Color GreenColor => greenColor;
    public Sprite RedBody => redBody;
    public Color RedColor => redColor;
    public SkillData[] Skills => skills;
    public int Health => health;
}
