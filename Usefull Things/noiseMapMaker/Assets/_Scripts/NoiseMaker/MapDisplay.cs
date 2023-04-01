using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer mapDisplay;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawTeture(Texture2D texture)
    {
        mapDisplay.sharedMaterial.mainTexture = texture;

        mapDisplay.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D textureColorMap)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = textureColorMap;
    }
}
