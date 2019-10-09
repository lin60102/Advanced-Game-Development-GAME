using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perlinnoise : MonoBehaviour
{
    public int depth = 20;
    public int width = 500;
    public int height = 500;

    public float scale = 20f;
    public float offsetX=100f;
    public float offsetY = 100f;

    private void Start()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
    }
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            offsetX = offsetX + 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            offsetX = offsetX - 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            offsetY = offsetY + 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            offsetY = offsetY - 1;
        }
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }


    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }
    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                heights[i, j] = CalHeight(i, j);

            }
        }
        return heights;
    }

        float CalHeight(int i, int j)
    {
        float xC = (float)i / width * scale+offsetX;
        float yC = (float)j / height * scale+offsetY;

        return Mathf.PerlinNoise(xC, yC);
    }

}
