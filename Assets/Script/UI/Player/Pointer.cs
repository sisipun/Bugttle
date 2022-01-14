using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pointer : MonoBehaviour
{
    [SerializeField] private TileBase pointerTile;

    private Tilemap tilemap;
    private int width;
    private int height;

    void Awake()
    {
        this.tilemap = GetComponent<Tilemap>();
    }

    public void Init(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void SetPosition(Vector2Int position)
    {
        tilemap.SetTile(((Vector3Int)position), pointerTile);
    }

    public void SetPath(List<Vector2Int> path)
    {
        foreach (Vector2Int position in path)
        {
            tilemap.SetTile(((Vector3Int)position), pointerTile);
        }
    }

    public void Clear()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap.SetTile(((Vector3Int)new Vector2Int(x, y)), null);
            }
        }
    }
}
