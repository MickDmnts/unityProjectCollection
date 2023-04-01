using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This should be a separate file 
[System.Serializable]
public struct TerrainType
{
    public string Name;
    [Range(0f, 1f)] public float Height;
    public Color Color;
    public GameObject[] objsToSpawn;
}

public class MapGeneration : MonoBehaviour
{
    public enum DisplayMode
    {
        Noise,
        Regions,
        Terrain,
    }

    public DisplayMode displayMode;

    //241 because its width - 1
    [SerializeField] int mapChunkSize = 241, scale;
    [SerializeField, Range(0f, 4f)] int LOD = 1;
    [SerializeField, Range(0.1f, 2)] float stepSize;
    [SerializeField] int octaves = 4;
    [SerializeField, Range(0, 1)]
    float persistence = 0.5f;
    [SerializeField] float lacunarity = 2;
    [SerializeField] int seed = 3456;
    [SerializeField] Vector2 perlinOffset;
    [SerializeField] TerrainType[] regions;
    [SerializeField] float heightMultiplyer = 10f;
    [SerializeField] AnimationCurve heightCurve;
    [SerializeField] bool blurTexture;
    [SerializeField, Range(0f, 5f)] int blurAmount;

    [SerializeField] bool spawnObjs = false;
    [SerializeField] bool scaleObjs = false;

    GameObject objsRoot;

    public void GenerateMapData()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, scale,
            seed,
            octaves, persistence, lacunarity,
            perlinOffset);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];

        if (objsRoot) DestroyImmediate(objsRoot);

        objsRoot = new GameObject()
        {
            name = "_Object Root",
        };

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].Height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].Color;

                        if (spawnObjs)
                        {
                            if (regions[i].objsToSpawn.Length > 0)
                            {
                                //Cenetr the map
                                float topLeftX = (mapChunkSize - 1) * stepSize / -2f;
                                float topLeftZ = (mapChunkSize - 1) * stepSize / 2f;

                                //Get the vertex position
                                Vector3 pos = new Vector3(topLeftX + x * stepSize, heightCurve.Evaluate(noiseMap[x, y]) * heightMultiplyer * stepSize, topLeftZ - y * stepSize);

                                //Add offset based on step size
                                pos += new Vector3(0.5f * stepSize, 0f, -0.5f * stepSize);

                                //Spawn object
                                GameObject spawnedObj = Instantiate(regions[i].objsToSpawn[Random.Range(0, regions[i].objsToSpawn.Length)], pos, Quaternion.identity, objsRoot.transform);

                                //Scale object
                                spawnedObj.transform.localScale = scaleObjs == false ? spawnedObj.transform.localScale : Vector3.one * stepSize;
                            }
                        }

                        break;
                    }
                }
            }
        }

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

        switch (displayMode)
        {
            case DisplayMode.Noise:
                mapDisplay.DrawTeture(TextureGenerator.TextureFromHeightMap(noiseMap));
                break;

            case DisplayMode.Regions:
                mapDisplay.DrawTeture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
                break;

            case DisplayMode.Terrain:
                Texture2D newTexture = TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize);

                if (blurAmount == 0) blurTexture = false;
                if (blurTexture) newTexture = TextureGenerator.Blur(newTexture, blurAmount);

                mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, heightMultiplyer, heightCurve, stepSize, LOD), newTexture);
                break;
        }

    }

    private void OnValidate()
    {
        //We make sure the number is even
        mapChunkSize /= 2;
        mapChunkSize *= 2;

        if (LOD > 0)
        {
            for (int i = 0; i < 6; i++)
            {
                if (mapChunkSize % LOD * 2 != 0)
                {
                    mapChunkSize = mapChunkSize + ((LOD * 2) - 2);
                }
            }
        }

        mapChunkSize += 1;

        if (blurAmount == 0) blurTexture = false;

        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;

        //GenerateMapData();
    }
}
