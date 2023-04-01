using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshData
{
    public Vector3[] vertices;
    public int[] trianges;
    public Vector2[] UVs;

    int triangleIndex = 0;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        trianges = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        UVs = new Vector2[meshWidth * meshHeight];

        triangleIndex = 0;
    }

    public void AddTriangles(int a, int b, int c)
    {
        trianges[triangleIndex] = a;
        trianges[triangleIndex + 1] = b;
        trianges[triangleIndex + 2] = c;

        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = trianges;
        mesh.uv = UVs;

        mesh.RecalculateNormals();

        return mesh;
    }

}

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplyer, AnimationCurve heightCurve, float stepSize, int LOD)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        //This moves the terrain generation to the middle of the 
        //terrain and not in the bottom right
        float topLeftX = (width - 1) * stepSize / -2f;
        float topLeftZ = (height - 1) * stepSize / 2f;

        //We need this expression because we can't divide by 0
        int meshSimplificationIncrement = (LOD == 0 ? 1 : LOD * 2);
        int verticesPerLine = ((width - 1) / meshSimplificationIncrement) + 1;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        for (int y = 0; y < height; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x += meshSimplificationIncrement)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x * stepSize, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplyer * stepSize, topLeftZ - y * stepSize);
                meshData.UVs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangles(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangles(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}
