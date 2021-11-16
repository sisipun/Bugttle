using UnityEngine;

public class BugGenerator : MonoBehaviour
{
    [SerializeField] private BugData[] bugs;

    private Map map;

    public void Init(Map map)
    {
        this.map = map;
    }

    public void Generate()
    {
        for (int x = 0; x < map.Size; x++)
        {
            int y = Random.Range(0, map.Size);
            Vector2Int position = new Vector2Int(x, y);
            BugData data = bugs[Random.Range(0, bugs.Length)];
            Bug bug = Instantiate<GameObject>(data.Prefub).GetComponent<Bug>();
            bug.Init(position, x < map.Size / 2 ? BugSide.GREEN : BugSide.RED, data);
            map.SetBug(position, bug);
        }
    }
}
