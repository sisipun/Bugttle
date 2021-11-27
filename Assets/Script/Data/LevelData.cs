using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/New Level")]
public class LevelData : ScriptableObject
{
    [SerializeField] private LevelType type;

    public LevelType Type => type;
}
