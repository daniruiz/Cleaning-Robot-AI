using System.Collections.Generic;
using UnityEngine;

public class CleanedSpaceMap
{
    private float gridWidth;
    private HashSet<Vector2Int> cleanedPositions = new HashSet<Vector2Int>();
    private Vector2 playerPosition = new Vector2(0, 0);

    public CleanedSpaceMap(float gridWidth)
    {
        this.gridWidth = gridWidth;
    }

    public void MovePlayerPosition(Vector2 movement)
    {
        playerPosition += movement;
        Vector2Int newPosition = Vector2ToPosition(playerPosition);
        cleanedPositions.Add(newPosition);
    }

    private Vector2Int Vector2ToPosition(Vector2 v)
    {
        int x = (int)(v.x / gridWidth);
        int y = (int)(v.y / gridWidth);
        return new Vector2Int(x, y);
    }

    public Vector2 GetPlayerPosition()
    {
        return playerPosition;
    }

    public bool IsGridCleaned(Vector2 grid)
    {
        return cleanedPositions.Contains(Vector2ToPosition(grid));
    }

    public int GetNumCleanedPositions()
    {
        return cleanedPositions.Count;
    }
}
