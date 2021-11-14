using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugGenerator : MonoBehaviour
{
    [SerializeField] private BugData[] bugs;

    private Map map;
    private Background background;

    public void Init(Map map, Background background)
    {
        this.map = map;
        this.background = background;
    }

    public void Generate()
    {
        for (int x = 0; x < map.Size; x++)
        {
            int y = Random.Range(0, map.Size);
            Vector2Int position = new Vector2Int(x, y);
            BugData data = bugs[Random.Range(0, bugs.Length)];
            Bug bug = Instantiate<GameObject>(
                data.Prefub,
                background.CellToWorld(new Vector2Int(position.x, position.y)),
                Quaternion.identity
            ).GetComponent<Bug>();
            bug.Init(position, x < map.Size / 2 ? Bug.Side.USER : Bug.Side.AI, data);
            map.SetBug(position, bug);
        }
    }

    public void Clear()
    {
        for (int x = 0; x < map.Size; x++)
        {
            for (int y = 0; y < map.Size; y++)
            {
                Bug bug = map.GetBug(x, y);
                if (bug != null)
                {
                    Destroy(bug.gameObject);
                    map.SetBug(x, y, null);
                }
            }
        }
    }
}
