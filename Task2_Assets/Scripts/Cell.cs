using UnityEngine;

public class Cell
{
    public bool isOccupied;
    public int gridX;
    public int gridY;
    public Vector3 worldPosition;

    public Cell(int _gridX, int _gridY, Vector3 _worldPosition, bool _isOccupied)
    {
        gridX = _gridX;
        gridY = _gridY;
        worldPosition = _worldPosition;
        isOccupied = _isOccupied;
    }
}