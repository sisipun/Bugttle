using UnityEngine;
using UnityEngine.Tilemaps;

public class Background : MonoBehaviour
{
    private Tilemap tilemap;
    private int size;

    void Awake()
    {
        this.tilemap = GetComponent<Tilemap>();
    }

    public void Init(Map map)
    {
        this.size = map.Size;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                tilemap.SetTile(((Vector3Int)position), map.GetCell(position).Tile);
            }
        }
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

    public Vector3 CellToWorld(Vector2Int position)
    {
        return tilemap.CellToWorld(((Vector3Int)position));
    }

    public Vector2Int WorldToCell(Vector3 position)
    {
        return ((Vector2Int)tilemap.WorldToCell(position));
    }
}
