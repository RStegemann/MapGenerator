using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    /// <summary>
    /// Generates a NoiseMap using PerlinNoise
    /// </summary>
    /// <param name="size"> Vector containing x and y size of the map </param>
    /// <param name="scale"> Scale to used zo zoom in or out of the noise </param>
    /// <param name="seed"> pnrg seed to use for this map </param>
    /// <param name="persistence"> Higher persistence increases the amplitude of the octaves, increasing roughness </param>
    /// <param name="lacunarity"> Frequency multiplier of the octaves </param>
    /// <param name="octaves"> Number of perlin noises to layer </param>
    /// <returns>2D float array with height values</returns>
    public static float[,] PerlinNoiseMap(Vector2Int size, float scale, int seed, float persistence, float lacunarity, int octaves)
    {
        float[,] noiseMap = new float[size.x, size.y];
        System.Random prng = new System.Random(seed);
        float xCenter = size.x / 2;
        float yCenter = size.y / 2;
        // Save max and min heights to map the range to 0-1f afterwards
        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        Vector2[] octaveOffsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++)
        {
            //Random high numbers for a a high range, but not too high since perlin noise might break on too high numbers
            float offsetX = prng.Next(-10000, 10000);
            float offsetZ = prng.Next(-10000, 10000);
            octaveOffsets[i] = new Vector2(offsetX, offsetZ);
        }

        for(int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int o = 0; o < octaves; o++)
                {
                    float sampleX = (x - xCenter) / scale * frequency + octaveOffsets[o].x;
                    float sampleY = (y - yCenter) / scale * frequency + octaveOffsets[o].y;

                    // Multiply by 2 and subtract 1 to get a range from -1 to 1
                    noiseHeight += (Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1) * amplitude; 
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxHeight)
                {
                    maxHeight = noiseHeight;
                }
                else if (noiseHeight < minHeight)
                {
                    minHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        // Scale numbers back to 0 to 1
        for(int x = 0; x < size.x; x++)
        {
            for(int z = 0; z < size.y; z++)
            {
                noiseMap[x, z] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, z]);
            }
        }

        return noiseMap;
    }

    /// <summary>
    /// Uses Spatial Subdivision to generate a heightmap.
    /// </summary>
    /// <param name="input"> Input Array with size (2k+1)^2 </param>
    /// <param name="rndRange"> Upper limit for random values </param>
    /// <returns> Heightmap with size (2(2k)+1)^2 </returns>
    public static float[,] SpatialSubdivision(int targetSize, int seed, float persistence)
    {
        int mapSize = 2;
        System.Random prng = new System.Random(seed);
        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        float amplitude = 1;
        float[,] input = new float[mapSize, mapSize];
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                input[i, j] = (float)prng.NextDouble();
                if (input[i, j] > maxHeight)
                {
                    maxHeight = input[i, j];
                }
                else if (input[i, j] < minHeight) minHeight = input[i, j];
                amplitude *= persistence;
            }
        }


        while(mapSize < targetSize)
        {
            int nb = 2 * (input.GetLength(1) - 1) + 1;
            float[,] output = new float[nb, nb];
            for (int x = 0; x < nb; ++x)
            {
                int inputX = x >> 1;
                for (int z = 0; z < nb; ++z)
                {
                    int inputZ = z >> 1;
                    output[x, z] = (input[inputX, inputZ] +
                        input[inputX + (x&1), inputZ] +
                        input[inputX, inputZ + (z&1)] +
                        input[inputX + (x&1), inputZ + (z&1)]) / 4;
                    if (((x & 1) == 1) || ((z & 1) == 1))
                    {
                        output[x, z] += Randomize(prng, amplitude);
                    }
                    if(output[x, z] > maxHeight)
                    {
                        maxHeight = output[x, z];
                    }else if(output[x,z] < minHeight)
                    {
                        minHeight = output[x, z];
                    }
                }
            }
            input = output;
            amplitude *= persistence;
            mapSize = input.GetLength(1);
        }

        for (int x = 0; x < input.GetLength(0); ++x)
        {
            for(int z = 0; z < input.GetLength(0); ++z)
            {
                input[x, z] = Mathf.InverseLerp(minHeight, maxHeight, input[x, z]);
            }
        }

        return input;
    }

    private static float Randomize(System.Random random, float amplitude)
    {
        return ((float)random.NextDouble() * 2 - 1) * amplitude;
    }

    /// <summary>
    /// Uses BinarySpacePartition to create a map containing rectangular rooms
    /// </summary>
    /// <param name="mapSize"> Vector2Int containing x and y size of the map </param>
    /// <param name="minRoomSize"> Integer determining minimum room size </param>
    /// <param name="maxRoomSize"> Integer determining maximum room size </param>
    /// <param name="seed"> Seed for prng operations </param>
    /// <returns> Float of the given map size containings 1s for walls, 0s for halls/rooms </returns>
    public static float[,] BinarySpacePartitionRooms(Vector2Int mapSize, int minRoomSize, int maxRoomSize, int seed, int roomAmount, int hallwaySize, bool randomizeHallwaySize)
    {
        BinarySpaceTree tree = new BinarySpaceTree(seed, mapSize, minRoomSize, maxRoomSize, roomAmount, hallwaySize, randomizeHallwaySize);
        //return tree.GetPartitionMap();
        return tree.GetDungeonMap();
    }
}
