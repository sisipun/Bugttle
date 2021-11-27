using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/New Card")]
public class CardData : ScriptableObject
{
    [SerializeField] private GameObject prefub;
    [SerializeField] private Sprite image;
    [SerializeField] private BugData bug;

    public GameObject Prefub => prefub;
    public Sprite Image => image;
    public BugData Brefub => bug;
}
