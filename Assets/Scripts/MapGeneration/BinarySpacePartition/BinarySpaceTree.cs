using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to create a binary tree splitting a rectangle into random partitions to place rooms of random sizes within
/// </summary>
public class BinarySpaceTree
{
    public static System.Random prng;
    public static int debugRoomCount = 0;
    public static int debugPartitionCounter = 0;

    private BinarySpaceNode root;
    private Vector2Int maxSize;

    /// <summary>
    /// Basic constructor
    /// </summary>
    /// <param name="seed"> Seed for the prng to be used within this tree </param>
    /// <param name="mapSize"> Vector2Int containing the width and height of the map </param>
    /// <param name="minRoomSize"> Minimum size of rooms placed within this tree </param>
    /// <param name="maxRoomSize"> Maximum size of rooms placed within this tree </param>
    /// <param name="maxRoomAmount"> Maximum amount of rooms to create. Might be less </param>
    public BinarySpaceTree(int seed, Vector2Int mapSize, int minRoomSize, int maxRoomSize, int maxRoomAmount, int hallwaySize, bool randomizeHallwaySize)
    {
        debugPartitionCounter = 0;
        debugRoomCount = 0;
        root = new BinarySpaceNode(new Vector2Int(0, 0), mapSize.x, mapSize.y);
        prng = new System.Random(seed);
        root.CreatePartitions(minRoomSize, maxRoomAmount - 1);
        root.CreateRoom(minRoomSize, maxRoomSize);
        root.CreateHallway(hallwaySize, randomizeHallwaySize);
        this.maxSize = mapSize;
    }

    /// <summary>
    /// Struct containing the bottom left anchorpoint of the room, as well as width and height
    /// </summary>
    public struct Room
    {
        public Room(Vector2Int pos, int w, int h)
        {
            bottomLeft = pos;
            width = w;
            height = h;
        }

        public Vector2Int bottomLeft;
        public int width;
        public int height;
    }

    /// <summary>
    /// Same as Room for now, but might be useful to have a different struct for future features
    /// </summary>
    public struct Hallway
    {
        public Hallway(Vector2Int pos, int w, int h)
        {
            bottomLeft = pos;
            width = w;
            height = h;
        }

        public Vector2Int bottomLeft;
        public int width;
        public int height;
    }

    /// <summary>
    /// Returns a float array containing 0s for rooms and halls and 1s for walls
    /// </summary>
    /// <returns> Float array with the size given in the constructor of the tree </returns>
    public float[,] GetDungeonMap()
    {
        float[,] roomMap = new float[maxSize.x, maxSize.y];
        for(int x = 0; x < roomMap.GetLength(0); x++)
        {
            for(int y = 0; y < roomMap.GetLength(1); y++)
            {
                roomMap[x, y] = 1;
            }
        }
        root.AddRoomToMap(ref roomMap);
        root.AddHallwaysToMap(ref roomMap);
        return roomMap;
    }

    public float[,] GetPartitionMap()
    {
        float[,] roomMap = new float[maxSize.x, maxSize.y];
        for (int x = 0; x < roomMap.GetLength(0); x++)
        {
            for (int y = 0; y < roomMap.GetLength(1); y++)
            {
                roomMap[x, y] = 0;
            }
        }
        root.AddPartitionToMap(ref roomMap);
        return roomMap;
    }
}
