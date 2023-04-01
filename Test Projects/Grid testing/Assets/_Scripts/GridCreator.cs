using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator
{
    int gridWidth;
    int gridHeight;
    float gridCellSize;
    int[,] gridArray;

    public GridCreator(int width,int height,float cellSize)
    {
        this.gridWidth = width;
        this.gridHeight = height;
        this.gridCellSize = cellSize;

        gridArray = new int[gridWidth, gridHeight];

        Debug.Log(gridWidth + " " + gridHeight);

        CycleArrayDimensions();
    }

    void CycleArrayDimensions()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                primitive.transform.position = GetWorldPosition(x, y);
            }
        }
    }

    Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * gridCellSize;
    }

}
