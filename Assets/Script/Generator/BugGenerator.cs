using UnityEngine;

public class BugGenerator : MonoBehaviour
{
    [SerializeField] private BugData[] bugs;
    [SerializeField] private BugPool pool;

    public void Generate(Map map, int bugsCount)
    {
        for (int y = 0; y < bugsCount; y++)
        {
            Vector2Int position = new Vector2Int(0, y);
            BugData data = bugs[Random.Range(0, bugs.Length)];
            Bug bug = pool.Get();
            bug.Init(position, BugSide.GREEN, data);
            map.SetBug(position, bug);
        }

        for (int y = 0; y < bugsCount; y++)
        {
            Vector2Int position = new Vector2Int(map.Size - 1, map.Size - 1 - y);
            BugData data = bugs[Random.Range(0, bugs.Length)];
            Bug bug = pool.Get();
            bug.Init(position, BugSide.RED, data);
            map.SetBug(position, bug);
        }
    }
}
