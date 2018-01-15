using System;
using System.Collections.Generic;
using UnityEngine;

public class CleanedSpaceMap
{
    private float gridWidth;
    private HashSet<Vector2Int> cleanedPositions = new HashSet<Vector2Int>();
    private Vector2 playerPosition;
    private float directionAngle;

    public CleanedSpaceMap(float gridWidth)
    {
        this.gridWidth = gridWidth;
        playerPosition = new Vector2(gridWidth / 2, gridWidth / 2);
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

    public bool IsLeftGridCleaned()
    {
        return IsGridCleaned(AdjacentGrid("Left"));
    }
    public bool IsFrontGridCleaned()
    {
        return IsGridCleaned(AdjacentGrid("Front"));
    }
    public bool IsRightGridCleaned()
    {
        return IsGridCleaned(AdjacentGrid("Right"));
    }

    public int GetNumCleanedPositions()
    {
        return cleanedPositions.Count;
    }


    private Vector2Int Vector2ToPosition(Vector2 v)
    {
        int x = (int)Math.Floor((v.x / gridWidth));
        int y = (int)Math.Floor((v.y / gridWidth));
        return new Vector2Int(x, y);
    }

    private Vector2 AdjacentGrid(String side)
    {
        Vector2Int position = Vector2ToPosition(playerPosition);
        int degreesDiference = side == "Left" ? -90
                                   : (side == "Front" ? 0 : 90);
        float x = (position.x * gridWidth) + (gridWidth / 2) + (gridWidth * (float)Math.Sin((Math.PI / 180) * (directionAngle + degreesDiference)));
        float y = (position.y * gridWidth) + (gridWidth / 2) + (gridWidth * (float)Math.Cos((Math.PI / 180) * (directionAngle + degreesDiference)));
        return new Vector2(x, y);
    }

    private bool IsGridCleaned(Vector2Int grid)
    {
        return cleanedPositions.Contains(grid);
    }

    private bool IsGridCleaned(Vector2 grid)
    {
        return IsGridCleaned(Vector2ToPosition(grid));
    }
}
