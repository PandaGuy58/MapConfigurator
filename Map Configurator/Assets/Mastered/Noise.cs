using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, float seedX, float seedY)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        if(scale <= 0)
        {
            scale = 0.0001f;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float sampleX = x / scale;
                float sampleY = y / scale;

                float perlinValue = Mathf.PerlinNoise(sampleX + seedX, sampleY + seedY);

                if(perlinValue < 0)
                {
                    perlinValue = perlinValue * -1;
                }

                noiseMap[x, y] = perlinValue; 
            }
        }

        return noiseMap;
    }
    // Start is called be,fore the first frame update
   // void Start()
  //  {
        
   // }

    // Update is called once per frame
 // //  void Update()
 //   {
        
  //  }
}
