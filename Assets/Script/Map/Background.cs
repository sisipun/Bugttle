using UnityEngine;
using UnityEngine.Tilemaps;

public class Background : MonoBehaviour
{
    [SerializeField] private Tilemap backTilemap;
    [SerializeField] private Tilemap frontTilemap;
    private int size;

    public void Init(Map map)
    {
        this.size = map.Size;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                backTilemap.SetTile(((Vector3Int)position), map.GetCell(position).BackTile);
                frontTilemap.SetTile(((Vector3Int)position), map.GetCell(position).FrontTile);
            }
        }
    }

    public void SetCell(Vector2Int position, Cell cell)
    {
        backTilemap.SetTile(((Vector3Int)position), cell.BackTile);
        frontTilemap.SetTile(((Vector3Int)position), cell.FrontTile);
    }

    public void Clear()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                backTilemap.SetTile(((Vector3Int)new Vector2Int(x, y)), null);
                frontTilemap.SetTile(((Vector3Int)new Vector2Int(x, y)), null);
            }
        }
    }

    public Vector3 CellToWorld(Vector2Int position)
    {
        return backTilemap.CellToWorld(((Vector3Int)position));
    }

    public Vector2Int WorldToCell(Vector3 position)
    {
        return ((Vector2Int)backTilemap.WorldToCell(position));
    }
}
