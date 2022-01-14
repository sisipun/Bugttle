using UnityEngine;
using UnityEngine.Tilemaps;

public class Background : MonoBehaviour
{
    [SerializeField] private Tilemap backTilemap;
    [SerializeField] private Tilemap frontTilemap;
    [SerializeField] private Camera mainCamera;
    private int width;
    private int height;

    public void Init(int width, int height)
    {
        this.width = width;
        this.height = height;
        Vector3 backgroundCenter = CellToWorld(new Vector2Int(width / 2, height / 2));
        mainCamera.transform.position = new Vector3(backgroundCenter.x, backgroundCenter.y, mainCamera.transform.position.z);
    }

    public void SetCell(Cell cell)
    {
        backTilemap.SetTile(((Vector3Int)cell.Position), cell.BackTile);
        frontTilemap.SetTile(((Vector3Int)cell.Position), cell.FrontTile);
    }

    public void Clear()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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
