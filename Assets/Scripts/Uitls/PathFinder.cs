using System.Collections.Generic;
using UnityEngine;

class PathFinder
{
    public static List<Vector2Int> Find(Cell[,] map, Vector2Int source, Vector2Int target, int maxCost)
    {
        if (source == target)
        {
            return new List<Vector2Int>();
        }

        Dictionary<Vector2Int, Vector2Int> path = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> cost = new Dictionary<Vector2Int, int>();
        SortedSet<KeyValuePair<Vector2Int, int>> points = new SortedSet<KeyValuePair<Vector2Int, int>>(new PointComparer());
        
        points.Add(new KeyValuePair<Vector2Int, int>(source, 0));
        cost.Add(source, 0);

        while (points.Count != 0)
        {
            KeyValuePair<Vector2Int, int> priorityPoint = points.Min;
            points.Remove(priorityPoint);

            Vector2Int point = priorityPoint.Key;
            if (point == target)
            {
                break;
            }

            List<Vector2Int> neighbors = GetNeighbors(map, point);
            foreach (Vector2Int neighbor in neighbors)
            {
                Cell neighborCell = map[neighbor.x, neighbor.y];
                int neighborCost = cost[point] + neighborCell.Cost;
                if (
                    !neighborCell.HasBug! 
                    && neighborCost <= maxCost 
                    && (cost.ContainsKey(neighbor) || neighborCost < cost[neighbor]))
                {
                    cost[neighbor] = neighborCost;
                    path[neighbor] = point;
                    points.Add(new KeyValuePair<Vector2Int, int>(neighbor, neighborCost + Distance(neighbor, target)));
                }
            }
        }

        return GetShortestPath(path, source, target);
    }

    private static List<Vector2Int> GetNeighbors(Cell[,] map, Vector2Int point)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        if (point.x - 1 >= 0)
        {
            neighbors.Add(new Vector2Int(point.x - 1, point.y));
        }
        if (point.x + 1 < map.GetLength(0))
        {
            neighbors.Add(new Vector2Int(point.x + 1, point.y));
        }
        if (point.y - 1 >= 0)
        {
            neighbors.Add(new Vector2Int(point.x, point.y - 1));
        }
        if (point.y + 1 < map.GetLength(1))
        {
            neighbors.Add(new Vector2Int(point.x, point.y + 1));
        }
        return neighbors;
    }

    private static int Distance(Vector2Int source, Vector2Int target)
    {
        return Mathf.Abs(target.x - source.x) + Mathf.Abs(target.y - source.y);
    }

    private static List<Vector2Int> GetShortestPath(Dictionary<Vector2Int, Vector2Int> path, Vector2Int source, Vector2Int target) {
        List<Vector2Int> shortestPath = new List<Vector2Int>();
        Vector2Int current = target;
        while (current != source)
        {
            shortestPath.Add(current);
            if (!path.ContainsKey(current)) 
            {
                return new List<Vector2Int>();
            }
            current = path[current];
        }
        
        shortestPath.Reverse();
        return shortestPath;
    }

    private class PointComparer : IComparer<KeyValuePair<Vector2Int, int>>
    {
        public int Compare(KeyValuePair<Vector2Int, int> x, KeyValuePair<Vector2Int, int> y)
        {
            return y.Value - x.Value;
        }
    }
}