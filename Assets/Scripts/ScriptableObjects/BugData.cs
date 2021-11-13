using UnityEngine;

[CreateAssetMenu(fileName = "NewBug", menuName = "Scriptable Objects/New Bug")]
public class BugData : ScriptableObject
{

    [SerializeField]
    private GameObject prefub;
    [SerializeField]
    private Sprite userBody;
    [SerializeField]
    private Sprite aiBody;
    [SerializeField]
    private int moveRange;
    [SerializeField]
    private int attackRange;
    [SerializeField]
    private int health;

    public GameObject Prefub => prefub;
    public Sprite UserBody => userBody;
    public Sprite AiBody => aiBody;
    public int MoveRange => moveRange;
    public int AttackRange => attackRange;
    public int Health => health;
}
