using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Cell", menuName = "Scriptable Objects/New Cell")]
public class CellData : ScriptableObject
{
    [SerializeField] private BaseCellEffect cellEffect;
    [SerializeField] private TileBase backTile;
    [SerializeField] private TileBase frontTile;
    [SerializeField] private int cost;


    public BaseCellEffect CellEffect => cellEffect;
    public TileBase BackTile => backTile;
    public TileBase FrontTile => frontTile;
    public int Cost => cost;
}