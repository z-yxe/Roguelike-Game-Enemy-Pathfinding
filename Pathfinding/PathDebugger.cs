using UnityEngine;
public class PathDebugger : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private LayerMask obstacleLayer;
    public int gridWidth = 50;
    public int gridHeight = 50;
    public float cellSize = 1f;
    public Vector3 gridOrigin = new Vector3(-25f, -25f);
    
    [Header("Visual Settings")]
    public bool showGrid = true;
    public bool showObstacles = true;
    public Color gridColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
    public Color obstacleColor = new Color(1f, 0f, 0f, 0.3f);

    private Grid grid;
    private Pathfinding pathfinding;

    private void Start()
    {
        pathfinding = new Pathfinding(gridWidth, gridHeight, cellSize, gridOrigin, obstacleLayer);
    }
    private bool CheckForObstacle(Vector3 worldPosition)
    {
        Vector2 size = new Vector2(cellSize * 0.5f, cellSize * 0.5f); // Pastikan sedikit lebih kecil dari cell
        return Physics2D.OverlapBox(worldPosition, size, 0, obstacleLayer);
    }

    private void OnDrawGizmos()
    {
        if (!showGrid) return;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 worldPos = gridOrigin + new Vector3(x * cellSize, y * cellSize);
                bool isObstacle = CheckForObstacle(worldPos);
                Gizmos.color = isObstacle && showObstacles ? obstacleColor : gridColor;
                Vector3 cellSize3D = new Vector3(cellSize, cellSize, 0);
                Gizmos.DrawWireCube(worldPos + cellSize3D * 0f, cellSize3D);
            }
        }
    }
}