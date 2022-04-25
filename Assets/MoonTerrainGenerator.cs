using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonTerrainGenerator : MonoBehaviour
{
    [SerializeField] private int depth;
    [SerializeField] private float scale;
    [SerializeField] private GameObject prefabBase;

    private int width = 512;
    private int height = 512;
    private int crater1X;
    private int crater1Y;
    private int crater2X;
    private int crater2Y;
    private int crater3;
    private int crater4;

    // Start is called before the first frame update
    private void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        crater1X = Random.Range(20, 200);
        crater1Y = Random.Range(20, 200);
        crater2X = Random.Range(250, 460);
        crater2Y = Random.Range(250, 460);
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        Smooth(terrain.terrainData);
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
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if((x > crater1X && x < crater1X + 20 && y > crater1Y && y < crater1Y + 20) || (x > crater2X && x < crater2X + 20 && y > crater2Y && y < crater2Y + 20))
                {
                    heights[x, y] = -1;
                }
                else
                {
                    heights[x, y] = CalculateHeight(x, y);
                }
            }
        }
        var crater1Base = Instantiate(prefabBase, new Vector3(crater1Y + 10, 0, crater1X + 10), Quaternion.identity);
        var crater2Base = Instantiate(prefabBase, new Vector3(crater2Y + 10, 0, crater2X + 10), Quaternion.identity);
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
