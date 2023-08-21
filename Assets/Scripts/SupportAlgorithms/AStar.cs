using System.Collections.Generic;
using UnityEngine;


public class AStar
{
    public TileManager tileManager;
    public Dictionary<Vector3Int, Vector3Int> cameFrom;
    public Dictionary<Vector3Int, double> costSoFar;
    
    
    public AStar(TileManager tManager)
    {
        tileManager = tManager;
    }
    public List<Vector3Int> AStarSearch(Vector3Int start, Vector3Int goal, int searchCapacity)
    {

        var frontier = new PriorityQueue<Vector3Int, double>(searchCapacity);
        frontier.Enqueue(start, 0);

        // Initialize
        cameFrom = new();
        costSoFar = new();
        cameFrom[start] = start;
        costSoFar[start] = 0;

        while(true)
        {
            var current = frontier.Dequeue();
            if(current == goal)
            {
                break;
            }
            foreach(var next in tileManager.Neighbors(current))
            {
                double newCost = costSoFar[current] + tileManager.Cost(current, next);
                if(!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    double priority = newCost + tileManager.Heuristic(next, goal);
                    frontier.Enqueue(next, -1*priority);
                    cameFrom[next] = current;
                }
            }
            if(frontier.Count == 0)
            {
                //Debug.Log("No Path found");
                return new List<Vector3Int>();
            }
        }

        return FoundPath(start, goal);
    }

    public List<Vector3Int> ReachSearch(Vector3Int start, List<Vector3Int> searchRange, int costCapacity)
    {
        List<Vector3Int> res = new();

        foreach(var pos in searchRange)
        {
            
            AStarSearch(start, pos, 1000);
            if(costSoFar[pos] <= costCapacity)
            {
                res.Add(pos);
            }
        }
        return res;
    }

    public List<Vector3Int> FoundPath(Vector3Int start, Vector3Int goal)
    {
        Vector3Int current = goal;
        List<Vector3Int> path = new();
        path.Add(goal);
        // Debug.Log("goal added");
        while (true)
        {
            if (cameFrom[current] == start)
            {
                // Debug.Log(start);
                // Debug.Log($"current is {current}, parent is {cameFrom[current]}");
                break;
            }

            current = cameFrom[current];
            path.Add(current);
            // Debug.Log($"{current} added");
        }
        path.Reverse();
        return path;
    }
}


