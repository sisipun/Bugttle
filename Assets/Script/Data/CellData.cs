using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Cell", menuName = "Scriptable Objects/New Cell")]
public class CellData : ScriptableObject
{
    [SerializeField] private BaseCellEffect cellEffect;
    [SerializeField] private TileBase backTile;
    [SerializeField] private TileBase frontTile;
    [SerializeField] private int cost;
    [SerializeField] private int frequency;


    public BaseCellEffect CellEffect => cellEffect;
    public TileBase BackTile => backTile;
    public TileBase FrontTile => frontTile;
    public int Cost => cost;
    public int Frequency => frequency;
}