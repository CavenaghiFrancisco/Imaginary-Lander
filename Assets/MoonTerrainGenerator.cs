using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonTerrainGenerator : MonoBehaviour
{
    [SerializeField] private int depth;
    [SerializeField] private float scale;
    [SerializeField] private GameObject prefabBase;

    private int width = 256;
    private int height = 256;
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
        crater1X = Random.Range(20, 50);
        crater1Y = Random.Range(20, 50);
        crater2X = Random.Range(20, 50);
        crater2Y = Random.Range(130, 200);
        crater3X = Random.Range(130, 200);
        crater3Y = Random.Range(20, 50);
        crater4X = Random.Range(130, 200);
        crater4Y = Random.Range(130, 200);
        offsetX = Random.Range(100, 9999);
        offsetY = Random.Range(100, 9999);
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
                if((x > crater1X && x < crater1X + 30 && y > crater1Y && y < crater1Y + 30) || (x > crater2X && x < crater2X + 30 && y > crater2Y && y < crater2Y + 30) || (x > crater3X && x < crater3X + 30 && y > crater3Y && y < crater3Y + 30) || (x > crater4X && x < crater4X + 30 && y > crater4Y && y < crater4Y + 30))
                {
                    heights[x, y] = -1;
                }
                else
                {
                    heights[x, y] = CalculateHeight(x, y);
                }
            }
        }
        var crater1Base = Instantiate(prefabBase, new Vector3(crater1Y + 15, 0, crater1X + 15), Quaternion.identity);
        var crater2Base = Instantiate(prefabBase, new Vector3(crater2Y + 15, 0, crater2X + 15), Quaternion.identity);
        var crater3Base = Instantiate(prefabBase, new Vector3(crater3Y + 15, 0, crater3X + 15), Quaternion.identity);
        var crater4Base = Instantiate(prefabBase, new Vector3(crater4Y + 15, 0, crater4X + 15), Quaternion.identity);
        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

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
