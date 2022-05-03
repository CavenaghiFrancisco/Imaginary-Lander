using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonTerrainGenerator : MonoBehaviour
{
    [SerializeField] private int depth;
    [SerializeField] private float scale;

    private int width = 512;
    private int height = 512;
    private int crater1X;
    private int crater1Y;
    private int crater2X;
    private int crater2Y;
    private int crater3X;
    private int crater3Y;
    private int crater4X;
    private int crater4Y;
    private int offsetX;
    private int offsetY;

    // Start is called before the first frame update
    private void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        Smooth(terrainData);
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                
                heights[x, y] = CalculateHeight(x, y);

            }
        }
        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }



    public void Smooth(TerrainData terrain)
    {
        int heightmapWidth = terrain.heightmapResolution;
        int heightmapHeight = terrain.heightmapResolution;
        float[,] heights = terrain.GetHeights(0, 0, heightmapWidth, heightmapHeight);
        Smooth(heights, terrain);
        terrain.SetHeights(0, 0, heights);
    }

    public void Smooth(float[,] heights, TerrainData terrain)
    {
        float[,] numArray = heights.Clone() as float[,];
        int length1 = heights.GetLength(1);
        int length2 = heights.GetLength(0);
        for (int index1 = 1; index1 < length2 - 1; ++index1)
        {
            for (int index2 = 1; index2 < length1 - 1; ++index2)
            {
                float num = (0.0f + numArray[index1, index2] + numArray[index1, index2 - 1] + numArray[index1, index2 + 1] + numArray[index1 - 1, index2] + numArray[index1 + 1, index2]) / 5f;
                heights[index1, index2] = num;
            }
        }
    }
}
