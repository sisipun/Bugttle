using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBug", menuName = "Scriptable Objects/Card Bug")]
public class BugData : ScriptableObject
{
    [SerializeField]
    private Sprite body;
    
    [SerializeField]
    private GameObject prefub;

    public Sprite Body => body;
    public GameObject Prefub => prefub;
}
