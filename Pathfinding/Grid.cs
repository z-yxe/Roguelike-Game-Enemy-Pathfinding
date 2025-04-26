using UnityEngine;
using System.Collections.Generic;

public class Grid
{
    private readonly int width;
    private readonly int height;
    private readonly float cellSize;
    private readonly Vector3 originPosition;
    private readonly Node[,] gridArray;
    private readonly LayerMask obstacleLayer; // Add layer mask for better performance

    public Grid(int width, int height, float cellSize, Vector3 originPosition, LayerMask obstacleLayer)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.obstacleLayer = obstacleLayer;

        gridArray = new Node[width, height];
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y);
                bool isWalkable = !CheckForObstacle(worldPosition);
                gridArray[x, y] = new Node(isWalkable, worldPosition, x, y);
            }
        }
    }

    private bool CheckForObstacle(Vector3 worldPosition)
    {
        Vector2 size = new Vector2(cellSize * 0.5f, cellSize * 0.5f); // Pastikan sedikit lebih kecil dari cell
        return Physics2D.OverlapBox(worldPosition, size, 0, obstacleLayer);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize, y * cellSize) + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.RoundToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.RoundToInt((worldPosition - originPosition).y / cellSize);
    }


    public Node GetNode(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < width && y < height) ? gridArray[x, y] : null;
    }

    public List<Node> GetNeighbours(Node node, bool allowDiagonal = true)
    {
        List<Node> neighbours = new List<Node>(8); // Pre-allocate capacity

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                if (!allowDiagonal && Mathf.Abs(x) + Mathf.Abs(y) == 2) continue;

                int checkX = node.x + x;
                int checkY = node.y + y;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    Node neighbor = gridArray[checkX, checkY];
                    if (allowDiagonal && Mathf.Abs(x) + Mathf.Abs(y) == 2)
                    {
                        // Check if corner cutting is possible
                        bool canMoveDiagonally = gridArray[node.x, checkY].isWalkable &&
                                               gridArray[checkX, node.y].isWalkable;
                        if (canMoveDiagonally)
                        {
                            neighbours.Add(neighbor);
                        }
                    }
                    else
                    {
                        neighbours.Add(neighbor);
                    }
                }
            }
        }
        return neighbours;
    }
}