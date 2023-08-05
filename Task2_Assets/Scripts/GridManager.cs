using System;
using UnityEngine;

public class GridManager : MonoBehaviour 
{
    internal Cell[,] grid;

    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float cellRadius;
    private int gridSizeX;
    private int gridSizeY;
    private float cellDiameter; 
    private Vector3 gridBottomLeft;

    void Start()
    {
        cellDiameter = cellRadius * 2;
        gridSizeX = Mathf.FloorToInt(gridWorldSize.x / cellDiameter);
        gridSizeY = Mathf.FloorToInt(gridWorldSize.y / cellDiameter);
        gridBottomLeft = transform.position - new Vector3(gridSizeX, gridSizeY, 0f) * 0.5f * cellDiameter;

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new Cell[gridSizeX, gridSizeY];

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                var worldPos = gridBottomLeft + Vector3.right * (cellDiameter * i + cellRadius) 
                    + Vector3.up * (cellDiameter * j + cellRadius);
                grid[i, j] = new Cell(i, j, worldPos, false);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, (Vector3)gridWorldSize);
        if (grid != null)
        {
            foreach ( Cell cell in grid)
            {
                if (cell.isOccupied) Gizmos.color = Color.red;
                else Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(cell.worldPosition, Vector3.one * cellDiameter * 0.85f);
            }
        }
    }
}