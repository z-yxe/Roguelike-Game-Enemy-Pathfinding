using UnityEngine;
using System.Collections.Generic;

public class Pathfinding
{
    public readonly Grid grid;
    private readonly List<Node> openSet;
    private readonly HashSet<Node> closedSet;
    private readonly BinaryHeap<Node> openHeap;

    // Initializes pathfinding system
    public Pathfinding(int width, int height, float cellSize, Vector3 originPosition, LayerMask obstacleLayer)
    {
        grid = new Grid(width, height, cellSize, originPosition, obstacleLayer);

        openSet = new List<Node>();
        closedSet = new HashSet<Node>();
        openHeap = new BinaryHeap<Node>(width * height);
    }

    // Executes A* algorithm
    public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos, bool allowDiagonal = true)
    {
        grid.GetXY(startPos, out int startX, out int startY);
        grid.GetXY(targetPos, out int targetX, out int targetY);

        Node startNode = grid.GetNode(startX, startY);
        Node targetNode = grid.GetNode(targetX, targetY);

        if (startNode == null || targetNode == null || !startNode.isWalkable || !targetNode.isWalkable)
            return null;

        openSet.Clear();
        closedSet.Clear();
        openHeap.Clear();

        startNode.ResetPathfindingData();
        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, targetNode);

        openSet.Add(startNode);
        openHeap.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openHeap.RemoveFirst();
            openSet.Remove(currentNode);

            if (currentNode == targetNode)
            {
                return SmoothPath(RetracePath(startNode, targetNode));
            }

            closedSet.Add(currentNode);

            foreach (Node neighbour in grid.GetNeighbours(currentNode, allowDiagonal))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                    continue;

                int newGCost = currentNode.gCost + CalculateDistance(currentNode, neighbour);

                if (newGCost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newGCost;
                    neighbour.hCost = CalculateDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                        openHeap.Add(neighbour);
                    }
                    else
                    {
                        openHeap.UpdateItem(neighbour);
                    }
                }
            }
        }
        return null;
    }

    // Reconstructs final path
    private List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.worldPosition);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    // Optimizes path waypoints
    private List<Vector3> SmoothPath(List<Vector3> path)
    {
        if (path == null || path.Count < 3)
            return path;

        List<Vector3> smoothedPath = new List<Vector3>();
        smoothedPath.Add(path[0]);

        int current = 0;
        while (current < path.Count - 2)
        {
            int furthest = current + 1;
            for (int i = current + 2; i < path.Count; i++)
            {
                if (IsPathClear(path[current], path[i]))
                {
                    furthest = i;
                }
            }
            smoothedPath.Add(path[furthest]);
            current = furthest;
        }

        if (current != path.Count - 1)
        {
            smoothedPath.Add(path[path.Count - 1]);
        }

        return smoothedPath;
    }

    // Checks for obstacles
    private bool IsPathClear(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        RaycastHit2D hit = Physics2D.Raycast(start, (end - start).normalized, distance, grid.obstacleLayer);
        return hit.collider == null;
    }

    // Calculates heuristic cost
    private int CalculateDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.x - b.x);
        int distY = Mathf.Abs(a.y - b.y);

        // Using 14 for diagonal, 10 for straight (avoids float math)
        return distX > distY ? 14 * distY + 10 * (distX - distY) : 14 * distX + 10 * (distY - distX);
    }
}
