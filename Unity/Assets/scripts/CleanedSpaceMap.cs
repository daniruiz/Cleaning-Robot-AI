using System;
using System.Collections.Generic;
using UnityEngine;

public class CleanedSpaceMap
{
    private float gridWidth;
    private HashSet<Vector2Int> cleanedPositions = new HashSet<Vector2Int>();
    private Vector2 playerPosition = new Vector2(0, 0);
    private float directionAngle;

    public CleanedSpaceMap(float gridWidth)
    {
        this.gridWidth = gridWidth;
    }

    public void MovePlayerPosition(float displacement)
    {
        float x = displacement * (float)Math.Sin((Math.PI / 180) * directionAngle);
        float y = displacement * (float)Math.Cos((Math.PI / 180) * directionAngle);
        playerPosition += new Vector2(x, y);
        Vector2Int newPosition = Vector2ToPosition(playerPosition);
        cleanedPositions.Add(newPosition);
    }

    public void UpdateDirectionAngle(float angle)
    {
        directionAngle = (directionAngle + angle) % 360;
    }

    private Vector2Int Vector2ToPosition(Vector2 v)
    {
        int x = (int)(v.x / gridWidth);
        int y = (int)(v.y / gridWidth);
        return new Vector2Int(x, y);
    }

    public bool IsLeftGridCleaned()
    {
        float x = playerPosition.x + (gridWidth * (float)Math.Sin((Math.PI / 180) * (directionAngle - 90)));
        float y = playerPosition.y + (gridWidth * (float)Math.Cos((Math.PI / 180) * (directionAngle - 90)));
        return IsGridCleaned(new Vector2(x, y));
    }
    public bool IsFrontGridCleaned()
    {
        float x = playerPosition.x + (gridWidth * (float)Math.Sin((Math.PI / 180) * directionAngle));
        float y = playerPosition.y + (gridWidth * (float)Math.Cos((Math.PI / 180) * directionAngle));
        return IsGridCleaned(new Vector2(x, y));
    }
    public bool IsRightGridCleaned()
    {
        float x = playerPosition.x + (gridWidth * (float)Math.Sin((Math.PI / 180) * (directionAngle + 90)));
        float y = playerPosition.y + (gridWidth * (float)Math.Cos((Math.PI / 180) * (directionAngle + 90)));
        return IsGridCleaned(new Vector2(x, y));
    }

    private bool IsGridCleaned(Vector2Int grid)
    {
        return cleanedPositions.Contains(grid);
    }

    private bool IsGridCleaned(Vector2 grid)
    {
        return IsGridCleaned(Vector2ToPosition(grid));
    }

    public int GetNumCleanedPositions()
    {
        return cleanedPositions.Count;
    }
}
