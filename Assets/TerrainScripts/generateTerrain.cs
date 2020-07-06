using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class generateTerrain : MonoBehaviour
{
    //This is on a terrain object
    //Which contains terrain data
    Terrain self;
    TerrainData thisTerrainData;

    void Start()
    {
        System.Random random = new System.Random();
        int terrainResolution = this.GetComponent<Terrain>().terrainData.heightmapResolution;
        float[,] newHeightmap = new float[terrainResolution, terrainResolution];

        //flat world
        for (int i = 0; i < terrainResolution; i++)
        {
            for (int j = 0; j < terrainResolution; j++)
            {
                newHeightmap[i, j] = 0f;
            }
        }

        int maxWidth = terrainResolution / 4;
        for (int i = 0; i < 10; i++)
        {
            int xWidth = random.Next(0, maxWidth);
            int xStart = random.Next(0, terrainResolution - xWidth + 1);
            int stripDirection = random.Next(0, 2);
            float raiseHeight = random.Next(1, 4);

            for (int j = xStart; j < xStart + xWidth; j++)
            {
                int yWidth = random.Next(0, maxWidth);
                int yStart = random.Next(0, terrainResolution - yWidth + 1);
                for (int k = yStart; k < yStart+yWidth; k++)
                {
                    if (stripDirection == 0)
                    {
                        newHeightmap[j, k] += raiseHeight;
                    }
                    else
                    {
                        newHeightmap[k, j] += raiseHeight;
                    }
                }
            }
        }

        newHeightmap = boxblur(newHeightmap);
        newHeightmap = boxblur(newHeightmap);

        int numTextures = 2;
        //Now do alphamap stuff at it
        float[,,] alphamap = new float[this.GetComponent<Terrain>().terrainData.alphamapWidth,
                                       this.GetComponent<Terrain>().terrainData.alphamapHeight, numTextures];

        for (int i = 0; i < terrainResolution - 1; i++)
        {
            for (int j = 0; j < terrainResolution - 1; j++)
            {
                alphamap[i, j, 0] = newHeightmap[i, j];
                alphamap[i, j, 1] = 1-newHeightmap[i, j];
            }
        }

        //Set the alpha maps
               this.GetComponent<Terrain>().terrainData.SetAlphamaps(0, 0, alphamap);
        //Set the heights
               this.GetComponent<Terrain>().terrainData.SetHeights(0, 0, newHeightmap);
               this.GetComponent<Terrain>().terrainData.SyncHeightmap();

    }

    float[,] boxblur(float[,] array)
    {
        float[,] newArray = array;

        int dist = 16;
        int length = newArray.GetLength(0);

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                float sumval = 0;
                int num = 0;
                for (int m = -dist; m < dist; m++)
                {
                    for (int n = -dist; n < dist; n++)
                    {
                        if (i+m > 0 && i+m < length - 1 && j+n > 0 && j+n < length - 1)
                        {
                            sumval += array[i + m, j + n];
                            num++;
                        }
                    }
                }
                newArray[i, j] = sumval / num;
            }
        }

        return newArray; 
    }
    
}

