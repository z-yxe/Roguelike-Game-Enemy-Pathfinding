using UnityEngine;

public class Node : System.IComparable<Node>
{
    public readonly bool isWalkable;
    public readonly Vector3 worldPosition;
    public readonly int x;
    public readonly int y;

    public int gCost;
    public int hCost;
    public Node parent;
    public int fCost => gCost + hCost;

    public float arrivalTime;

    public Node(bool isWalkable, Vector3 worldPosition, int x, int y)
    {
        this.isWalkable = isWalkable;
        this.worldPosition = worldPosition;
        this.x = x;
        this.y = y;

        ResetPathfindingData();
    }

    public void ResetPathfindingData()
    {
        gCost = int.MaxValue;
        hCost = 0;
        parent = null;
        arrivalTime = 0;
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }
        return compare;
    }
}