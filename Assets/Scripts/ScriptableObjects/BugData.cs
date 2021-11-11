using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBug", menuName = "Scriptable Objects/Card Bug")]
public class BugData : ScriptableObject
{

    [SerializeField]
    private GameObject prefub;
    [SerializeField]
    private Sprite body;
    [SerializeField]
    private int turnRange;
    [SerializeField]
    private int attackRange;
    [SerializeField]
    private int health;

    public GameObject Prefub => prefub;
    public Sprite Body => body;
    public int TurnRange => turnRange;
    public int AttackRange => attackRange;
    public int Health => health;
}
