using System;
using UnityEngine;

public class generateTerrain : MonoBehaviour
{
    //This is on a terrain object
    //Which contains terrain data
    Terrain oldTerrain;
    TerrainData thisTerrainData;

    void Start()
    {
        oldTerrain = this.GetComponent<Terrain>();
        int terrainResolution = oldTerrain.terrainData.heightmapResolution;


        float[,] newHeightmap = generateHeightmap(terrainResolution);
        oldTerrain.terrainData.SetHeights(0, 0, newHeightmap);
        oldTerrain.terrainData.SyncHeightmap();

        oldTerrain.terrainData.SetHeights(0, 0, newHeightmap);
        oldTerrain.terrainData.SyncHeightmap();


        //TEXTURES
        int numTextures = 2;
        //Now do alphamap stuff at it
        float[,,] alphamap = new float[oldTerrain.terrainData.alphamapWidth,
                                       oldTerrain.terrainData.alphamapHeight, numTextures];

        for (int i = 0; i < terrainResolution - 1; i++)
        {
            for (int j = 0; j < terrainResolution - 1; j++)
            {
                alphamap[i, j, 0] = newHeightmap[i, j];
                alphamap[i, j, 1] = 1-newHeightmap[i, j];
            }
        }
        //Set the alpha maps
        oldTerrain.terrainData.SetAlphamaps(0, 0, alphamap);


        //Generate grass texture -- THIS MUST HAPPEN AFTER HEIGHTS ARE SET IN ORDER TO CALCULATE STEEPNESS
        int[,] grassLayer = oldTerrain.terrainData.GetDetailLayer(0, 0, oldTerrain.terrainData.detailHeight, oldTerrain.terrainData.detailWidth, 0);
        oldTerrain.terrainData.SetDetailLayer(0, 0, 0, generateGrass(grassLayer, oldTerrain.terrainData));
    }

    float[,] generateHeightmap(int terrainResolution)
    {
        System.Random random = new System.Random();

        float[,] newHeightmap = new float[terrainResolution, terrainResolution];


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
        return newHeightmap;
        
        //Set the heights

    }

    int[,] generateGrass(int[,] grassLayerToGenerate, TerrainData terrainData)
    {
        for (int i = 0; i < terrainData.detailHeight; i++)
        {
            for (int j = 0; j < terrainData.detailWidth; j++)
            {
                float normi = (float)((i / 2) * 1.0 / (terrainData.alphamapHeight - 1));
                float normj = (float)((j / 2) * 1.0 / (terrainData.alphamapWidth - 1));

                float steepness = Math.Abs(terrainData.GetSteepness(normi, normj));

                if (steepness > 50)
                {
                    grassLayerToGenerate[j, i] = 0;
                }
                else
                {
                    grassLayerToGenerate[j, i] = (int)(10 - (steepness / 5));
                }

            }
        }
        return grassLayerToGenerate;
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

