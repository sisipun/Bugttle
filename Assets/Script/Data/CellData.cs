using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Cell", menuName = "Scriptable Objects/New Cell")]
public class CellData : ScriptableObject
{
    [SerializeField] private BaseCellEffect cellEffect;
    [SerializeField] private TileBase tile;
    [SerializeField] private int cost;


    public BaseCellEffect CellEffect => cellEffect;
    public TileBase Tile => tile;
    public int Cost => cost;
}