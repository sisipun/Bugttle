using UnityEngine;

[CreateAssetMenu(fileName = "NewBug", menuName = "Scriptable Objects/New Bug")]
public class BugData : ScriptableObject
{
    [SerializeField] private GameObject prefub;
    [SerializeField] private Sprite userBody;
    [SerializeField] private Color userColor;
    [SerializeField] private Sprite greenBody;
    [SerializeField] private Color greenColor;
    [SerializeField] private Sprite aiBody;
    [SerializeField] private Color aiColor;
    [SerializeField] private Sprite redBody;
    [SerializeField] private Color redColor;
    [SerializeField] private int moveRange;
    [SerializeField] private int attackRange;
    [SerializeField] private int health;

    public GameObject Prefub => prefub;
    public Sprite UserBody => userBody;
    public Color UserColor => userColor;
    public Sprite AiBody => aiBody;
    public Color AiColor => aiColor;
    public Sprite GreenBody => greenBody;
    public Color GreenColor => greenColor;
    public Sprite RedBody => redBody;
    public Color RedColor => redColor;
    public int MoveRange => moveRange;
    public int AttackRange => attackRange;
    public int Health => health;
}
