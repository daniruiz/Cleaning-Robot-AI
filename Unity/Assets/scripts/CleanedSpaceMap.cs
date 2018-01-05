﻿using System.Collections.Generic;
using UnityEngine;

public class cleanedSpaceMap
{
    private float gridWidth;
    private HashSet<Vector2Int> cleanedPositions = new HashSet<Vector2Int>();
    private Vector2 playerPosition = new Vector2(0, 0);

    public cleanedSpaceMap(float gridWidth)
    {
        this.gridWidth = gridWidth;
    }

    public void movePlayerPosition(Vector2 movement)
    {
        playerPosition += movement;
        int x = (int)(playerPosition.x / gridWidth);
        int y = (int)(playerPosition.y / gridWidth);
        Vector2Int newPosition = new Vector2Int(x, y);
        if (cleanedPositions.Add(newPosition))
            Debug.Log(newPosition);
    }

    public int getNumCleanedPositions()
    {
        return cleanedPositions.Count;
    }
}
