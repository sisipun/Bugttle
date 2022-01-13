using UnityEngine;
using UnityEngine.Pool;

public class BugPool : MonoBehaviour
{
    [SerializeField] private GameObject prefub;
    [SerializeField] private int initialCount;
    private ObjectPool<Bug> pool;

    void Awake()
    {
        pool = new ObjectPool<Bug>(
            () => Instantiate<GameObject>(prefub).GetComponent<Bug>(),
            (bug) => bug.gameObject.SetActive(true), 
            (bug) => bug.gameObject.SetActive(false), 
            (bug) => Destroy(bug), 
            false, 
            initialCount
        );
        Bug[] initialBugs = new Bug[initialCount];
        for (int i = 0; i < initialCount; i++)
        {
            initialBugs[i] = pool.Get();
        }
        for (int i = 0; i < initialCount; i++)
        {
            pool.Release(initialBugs[i]);
        }
    }

    public Bug Get()
    {
        return pool.Get();
    }

    public void Release(Bug bug)
    {
        pool.Release(bug);
    }
}
