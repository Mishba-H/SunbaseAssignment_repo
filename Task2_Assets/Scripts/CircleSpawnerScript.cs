using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawnerScript : MonoBehaviour
{
    private Cell[,] grid;

    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private float circleSizeMin;
    [SerializeField] private float circleSizeMax;
    const int minCircleCount = 5;
    const int maxCircleCount = 10;

    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask occupiedLayer;

    private int circleCount;

    void Awake()
    {
    }

    void Start()
    {
        grid = GetComponent<GridManager>().grid;
        circleCount = Random.Range(minCircleCount, maxCircleCount + 1);
        Debug.Log(circleCount);

        while (circleCount > 0)
        {
            int i = Random.Range(0, grid.GetLength(0));
            int j = Random.Range(0, grid.GetLength(1));
            float circleSize = Random.Range(circleSizeMin, circleSizeMax);
            
            GameObject circle = Instantiate(circlePrefab, grid[i, j].worldPosition, Quaternion.identity);
            circle.transform.localScale *= circleSize;
            circle.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0, 1, 0.7f, 1, 1, 1);

            if (!grid[i, j].isOccupied) 
            {
                circleCount--;
                ResetCellStatus();

                Debug.Log(i + ", " + j + "successful");
            }
            else 
            {
                Destroy(circle);

                Debug.Log(i + ", " + j + "failed");
            }
        }
    }

    void ResetCellStatus()
    {
        foreach (Cell cell in grid)
        {
            cell.isOccupied = Physics2D.OverlapCircle(cell.worldPosition, checkRadius, occupiedLayer);
        }
    }
}
