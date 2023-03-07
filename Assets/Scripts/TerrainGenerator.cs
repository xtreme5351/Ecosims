using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainControllers
{
    public class TerrainGenerator : MonoBehaviour
    {
        public int depth = 30;
        public int height = 125;
        public int width = 125;
        public float scale = 10f;
        public float Xoffset = 100f;
        public float Yoffset = 100f;


        void Start()
        {
            depth = 2;//Random.Range(0, 20);
            scale = 2;//Random.Range(0, 20);
            Xoffset = 2;//Random.Range(0, 99f);
            Yoffset = 2;//Random.Range(0, 99f);
            Terrain terrain = GetComponent<Terrain>();
            terrain.terrainData = generateTerrainData(terrain.terrainData);
        }

        TerrainData generateTerrainData(TerrainData inputTerrainData)
        {
            inputTerrainData.heightmapResolution = width;
            inputTerrainData.size = new Vector3(width, depth, height);
            inputTerrainData.SetHeights(0, 0, generateHeights());
            return inputTerrainData;
        }

        float[,] generateHeights()
        {
            float[,] heightData = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heightData[x, y] = calculateHeight(x, y);
                }
            }
            return heightData;
        }

        float calculateHeight(int x, int y)
        {
            float xCoord = (float)x / width * scale + Xoffset;
            float yCoord = (float)y / height * scale + Yoffset;
            return Mathf.PerlinNoise(xCoord, yCoord);
        }
    }
}