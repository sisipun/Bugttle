using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hover : MonoBehaviour
{
    [SerializeField] private TileBase moveTile;
    [SerializeField] private TileBase attackTile;

    private Tilemap tilemap;
    private int size;

    void Awake()
    {
        this.tilemap = GetComponent<Tilemap>();
    }
    
    public void Init(Map map)
    {
        this.size = map.Size;
    }

    public void SetMovable(ICollection<Vector2Int> positions)
    {
        Set(positions, moveTile);
    }

    public void SetAttackable(ICollection<Vector2Int> positions)
    {
        Set(positions, attackTile);
    }

    public void Clear()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                tilemap.SetTile(((Vector3Int)new Vector2Int(x, y)), null);
            }
        }
    }

    private void Set(ICollection<Vector2Int> positions, TileBase tile)
    {
        foreach (Vector2Int position in positions)
        {
            tilemap.SetTile(((Vector3Int)position), tile);
        }
    }
}
