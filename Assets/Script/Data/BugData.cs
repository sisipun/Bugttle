using UnityEngine;

[CreateAssetMenu(fileName = "Bug", menuName = "Scriptable Objects/New Bug")]
public class BugData : ScriptableObject
{
    [SerializeField] private GameObject prefub;
    [SerializeField] private Sprite bottomBody;
    [SerializeField] private Sprite topBody;
    [SerializeField] private SkillData[] skills;
    [SerializeField] private int health;

    public GameObject Prefub => prefub;
    public Sprite BottomBody => bottomBody;
    public Sprite TopBody => topBody;
    public SkillData[] Skills => skills;
    public int Health => health;
}
