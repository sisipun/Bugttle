using UnityEngine;
using UnityEngine.Tilemaps;

public class Pointer : MonoBehaviour
{
    [SerializeField] private TileBase pointerTile;

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

    public void SetPosition(Vector2Int position)
    {
        tilemap.SetTile(((Vector3Int)position), pointerTile);
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
}
