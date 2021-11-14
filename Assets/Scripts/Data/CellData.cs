using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewCell", menuName = "Scriptable Objects/New Cell")]
public class CellData : ScriptableObject
{
    [SerializeField]
    private TileBase tile;
    [SerializeField]
    private int cost;

    public TileBase Tile => tile;
    public int Cost => cost;
}