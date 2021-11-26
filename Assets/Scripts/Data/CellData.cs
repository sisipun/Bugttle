using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewCell", menuName = "Scriptable Objects/New Cell")]
public class CellData : ScriptableObject
{
    [SerializeField] private CellBehaviour cellBehaviour;
    [SerializeField] private TileBase tile;
    [SerializeField] private int cost;


    public CellBehaviour CellBehaviour => cellBehaviour;
    public TileBase Tile => tile;
    public int Cost => cost;
}