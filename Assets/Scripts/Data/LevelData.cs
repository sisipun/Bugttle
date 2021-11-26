using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Scriptable Objects/New Level")]
public class LevelData : ScriptableObject
{
    [SerializeField] private LevelType type;

    public LevelType Type => type;
}
