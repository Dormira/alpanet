using System;
using UnityEngine;

public class generateTerrain : MonoBehaviour
{
    //This is on a terrain object
    //Which contains terrain data

    void Start()
    {
        TerrainData terrainData = this.GetComponent<Terrain>().terrainData;
        int terrainResolution = terrainData.heightmapResolution;

        terrainData.SetHeights(0, 0, generateHeightmap(terrainResolution));
        terrainData.SyncHeightmap();


        //TEXTURES
        int numTextures = 2;
        //Now do alphamap stuff at it

        //8/13/2020 - Tried calling SetAlphamaps on each "pixel" of the alphamap individually
        //This made all of my problems much worse, so don't do that
        float[,,] alphamap = new float[terrainData.alphamapWidth,
                                       terrainData.alphamapHeight, numTextures];

        for (int i = 0; i < terrainResolution - 1; i++)
        {
            for (int j = 0; j < terrainResolution - 1; j++)
            {
                alphamap[i, j, 0] = terrainData.GetHeight(j, i)/terrainData.alphamapHeight;
                alphamap[i, j, 1] = 1-terrainData.GetHeight(j, i) / terrainData.alphamapHeight;
            }
        }
        //Set the alpha maps
        terrainData.SetAlphamaps(0, 0, alphamap);


        //Generate grass texture -- THIS MUST HAPPEN AFTER HEIGHTS ARE SET IN ORDER TO CALCULATE STEEPNESS
        int[,] grassLayer = terrainData.GetDetailLayer(0, 0,terrainData.detailHeight, terrainData.detailWidth, 0);
        generateGrass(ref grassLayer, ref terrainData);
        terrainData.SetDetailLayer(0, 0, 0, grassLayer);
    }

    float[,] generateHeightmap(int terrainResolution)
    {
        System.Random random = new System.Random();
        //maybe we'd be better served by setting the old one to zero
        float[,] newHeightmap = new float[terrainResolution, terrainResolution];


        int maxWidth = terrainResolution / 4;
        int xWidth, yWidth, xStart, yStart, stripDirection;
        float raiseHeight;
        for (int i = 0; i < 10; i++)
        {
            xWidth = random.Next(0, maxWidth);
            xStart = random.Next(0, terrainResolution - xWidth + 1);
            stripDirection = random.Next(0, 2);
            raiseHeight = random.Next(1, 4);

            for (int j = xStart; j < xStart + xWidth; j++)
            {
                yWidth = random.Next(0, maxWidth);
                yStart = random.Next(0, terrainResolution - yWidth + 1);
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

        boxblur(ref newHeightmap);
        boxblur(ref newHeightmap);
        return newHeightmap;

    }

    void generateGrass(ref int[,] grassLayerToGenerate, ref TerrainData terrainData)
    {
        //Do the grass layer modifications in place to save memory
        float normi;
        float normj;
        float steepness;
        for (int i = 0; i < terrainData.detailHeight; i++)
        {
            for (int j = 0; j < terrainData.detailWidth; j++)
            {
                normi = (float)((i / 2) * 1.0 / (terrainData.alphamapHeight - 1));
                normj = (float)((j / 2) * 1.0 / (terrainData.alphamapWidth - 1));

                steepness = Math.Abs(terrainData.GetSteepness(normi, normj));

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
    }

    void boxblur(ref float[,] array)
    {
        //Not exactly a boxblur anymore because we're not doing our blur on a fresh copy of array
        //But it still smooths the contours of our heightmap without killing webgl
        int dist = 16;
        int length = array.GetLength(0);
        float sumval;
        int num;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                sumval = 0;
                num = 0;
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
                array[i, j] = sumval / num;
            }
        }
    }
    
}

