using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BinarySpaceTree;

public class BinarySpaceNode
{
    private Vector2Int bottomLeft;
    private int width;
    private int height;

    private Room room;
    private List<Hallway> hallways;

    private BinarySpaceNode left;
    private BinarySpaceNode right;

    public int DebugId { get; }

    public BinarySpaceNode(Vector2Int anchor, int w, int h)
    {
        left = null;
        right = null;

        bottomLeft = anchor;
        width = w;
        height = h;

        room = new Room(new Vector2Int(-1, -1), 0, 0);
        hallways = new List<Hallway>();
        DebugId = debugPartitionCounter;
        debugPartitionCounter++;
    }

    public bool IsLeaf()
    {
        return (left == null && right == null);
    }

    /// <summary>
    /// Attempts to split the node into two nodes which can fit rooms of the given sizes
    /// </summary>
    /// <param name="minSize"> Minimum room size </param>
    /// <returns> True if split is successful, false if node can't be split anymore </returns>
    public bool Split(int minSize)
    {
        minSize++;
        // Return if the node already has children or is too small to fit a room in children
        if (!IsLeaf() || ((Mathf.Max(width, height)/2) < minSize))
        {
            return false;
        }

        bool horizontal = (prng.NextDouble() < 0.5f);
        if(width / 2 < minSize)
        {
            horizontal = true;
        }else if(height / 2 < minSize)
        {
            horizontal = false;
        }


        if (horizontal)
        {
            int split = prng.Next(minSize, height - minSize);
            left = new BinarySpaceNode(bottomLeft, width, split);
            right = new BinarySpaceNode(new Vector2Int(bottomLeft.x, bottomLeft.y + split), width, height - split);
        }
        else
        {
            int split = prng.Next(minSize, width - minSize);
            left = new BinarySpaceNode(bottomLeft, split, height);
            right = new BinarySpaceNode(new Vector2Int(bottomLeft.x + split, bottomLeft.y), width - split, height);
        }

        return true;
    }

    /// <summary>
    /// Recursively split self and children
    /// </summary>
    public int CreatePartitions(int minRoomSize, int roomAmount)
    {
        int created = 0;
        if (IsLeaf() && (roomAmount > 0))
        {
            if(Split(minRoomSize))
            {
                roomAmount--;
                int leftRooms = roomAmount / 2;

                created += left.CreatePartitions(minRoomSize, leftRooms);
                created += right.CreatePartitions(minRoomSize, roomAmount - leftRooms);
            }
        }
        return created;
    }

    /// <summary>
    /// Create Room if Node is Leaf, otherwise try to create nodes in Children
    /// </summary>
    /// <param name="minRoomSize"> Minimum size of a room </param>
    /// <param name="maxRoomSize"> Maximum size of a room </param>
    public void CreateRoom(int minRoomSize, int maxRoomSize)
    {
        if(left != null)
        {
            left.CreateRoom(minRoomSize, maxRoomSize);
        }
        if(right != null)
        {
            right.CreateRoom(minRoomSize, maxRoomSize);
        }
        if (IsLeaf())
        {
            debugRoomCount++;
            int roomWidth = prng.Next(minRoomSize, Mathf.Min(maxRoomSize+1, width));
            int roomHeight = prng.Next(minRoomSize, Mathf.Min(maxRoomSize+1, height));

            int roomX = prng.Next(1, width - roomWidth) + bottomLeft.x;
            int roomY = prng.Next(1, height - roomHeight) + bottomLeft.y;
            room = new Room(new Vector2Int(roomX, roomY), roomWidth, roomHeight);
        }
    }

    public Room GetRoom()
    {
        if (IsLeaf())
        {
            return room;
        }
        if (left != null)
        {
            return left.GetRoom();
        }
        if(right != null)
        {
            return right.GetRoom();
        }

        return new Room(new Vector2Int(-1, -1), 0, 0);
    }

    public List<Hallway> GetHallway()
    {
        if (!IsLeaf())
        {
            if (hallways.Count > 0) return hallways;
        }
        if (left != null)
        {
            return left.GetHallway();
        }
        if (right != null)
        {
            return right.GetHallway();
        }

        return hallways;
    }

    public void CreateHallway(int hallwaySize, bool randomizeHallwaySize)
    {
        if (left != null) left.CreateHallway(hallwaySize, randomizeHallwaySize);
        if (right != null) right.CreateHallway(hallwaySize, randomizeHallwaySize);
        if (!IsLeaf())
        {
            Room lRoom = left.GetRoom();
            Room rRoom = right.GetRoom();

            Vector2Int leftStart = new Vector2Int(prng.Next(lRoom.bottomLeft.x + 1, lRoom.bottomLeft.x + lRoom.width - 1), prng.Next(lRoom.bottomLeft.y + 1, lRoom.bottomLeft.y + lRoom.height - 1));
            Vector2Int rightStart = new Vector2Int(prng.Next(rRoom.bottomLeft.x + 1, rRoom.bottomLeft.x + rRoom.width - 1), prng.Next(rRoom.bottomLeft.y + 1, rRoom.bottomLeft.y + rRoom.height - 1));

            int deltaW = rightStart.x - leftStart.x;
            int deltaH = rightStart.y - leftStart.y;

            // Adding + 1 to all draw widths/heights to ensure the target field is covered as well
            if (deltaW != 0)
            {
                // Pick randomly whether to start horizontally first
                if (prng.NextDouble() > 0.5f) // Horizontal first
                {
                    if (deltaW > 0)
                    {
                        hallways.Add(new Hallway(leftStart, deltaW + 1, randomizeHallwaySize ? prng.Next(1, hallwaySize + 1) : hallwaySize));
                    }
                    else
                    {
                        hallways.Add(new Hallway(new Vector2Int(leftStart.x + deltaW, leftStart.y), Mathf.Abs(deltaW) + 1, randomizeHallwaySize ? prng.Next(1, hallwaySize + 1) : hallwaySize));
                    }
                    if (deltaH > 0) // Go up
                    {
                        hallways.Add(new Hallway(new Vector2Int(leftStart.x + deltaW, leftStart.y), randomizeHallwaySize ? prng.Next(1, hallwaySize + 1) : hallwaySize, deltaH + 1));
                    }
                    else // Go down
                    {
                        hallways.Add(new Hallway(new Vector2Int(leftStart.x + deltaW, leftStart.y + deltaH), randomizeHallwaySize ? prng.Next(1, hallwaySize + 1) : hallwaySize, Mathf.Abs(deltaH) + 1));
                    }
                }
                else // Vertical first
                {
                    if (deltaH > 0)
                    {
                        hallways.Add(new Hallway(leftStart, randomizeHallwaySize ? prng.Next(1, hallwaySize + 1) : hallwaySize, deltaH + 1));
                    }
                    else
                    {
                        hallways.Add(new Hallway(new Vector2Int(leftStart.x, leftStart.y + deltaH), randomizeHallwaySize ? prng.Next(1, hallwaySize + 1) : hallwaySize, Mathf.Abs(deltaH) + 1));
                    }
                    if (deltaW > 0)
                    {
                        hallways.Add(new Hallway(new Vector2Int(leftStart.x, leftStart.y + deltaH), deltaW + 1, randomizeHallwaySize ? prng.Next(1, hallwaySize + 1) : hallwaySize));
                    }
                    else
                    {
                        hallways.Add(new Hallway(new Vector2Int(leftStart.x + deltaW, leftStart.y + deltaH), Mathf.Abs(deltaW) + 1, randomizeHallwaySize ? prng.Next(1, hallwaySize + 1) : hallwaySize));
                    }
                }
            }
            else
            {
                if (deltaH > 0)
                {
                    hallways.Add(new Hallway(leftStart, randomizeHallwaySize ? prng.Next(1, hallwaySize + 1) : hallwaySize, deltaH + 1));
                }
                else
                {
                    hallways.Add(new Hallway(new Vector2Int(leftStart.x, leftStart.y + deltaH), randomizeHallwaySize ? prng.Next(1, hallwaySize + 1) : hallwaySize, Mathf.Abs(deltaH) + 1));
                }
            }
        }
    }

    public void AddRoomToMap(ref float[,] map)
    {
        if(left != null)
        {
            left.AddRoomToMap(ref map);
        }
        if(right != null)
        {
            right.AddRoomToMap(ref map);
        }
        if (IsLeaf())
        {
            for(int x = room.bottomLeft.x; x < room.bottomLeft.x + room.width; x++)
            {
                for(int y = room.bottomLeft.y; y < room.bottomLeft.y + room.height; y++)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    /// <summary>
    /// Adds hallways to map. Checks for xSize/ySize in case of bigger hallways or randomized hallway sizes to avoid IndexOutOfRange.
    /// Easier to implement here than checking in hallwaygeneration.
    /// </summary>
    /// <param name="map"></param>
    public void AddHallwaysToMap(ref float[,] map)
    {
        int xSize = map.GetLength(0);
        int ySize = map.GetLength(1);
        if(left != null)
        {
            left.AddHallwaysToMap(ref map);
        }
        if(right != null)
        {
            right.AddHallwaysToMap(ref map);
        }
        foreach (Hallway h in hallways)
        {
            for (int x = h.bottomLeft.x; x < h.bottomLeft.x + h.width; x++)
            {
                if (x > xSize - 1) continue;
                for (int y = h.bottomLeft.y; y < h.bottomLeft.y + h.height; y++)
                {
                    if (y > ySize - 1) continue;
                    map[x, y] = 0;
                }
            }
        }
    }

    /// <summary>
    /// Debug function to visualize partitions instead of rooms
    /// </summary>
    /// <param name="map"></param>
    public void AddPartitionToMap(ref float[,] map)
    {
        if (left != null)
        {
            left.AddPartitionToMap(ref map);
        }
        if(right != null)
        {
            right.AddPartitionToMap(ref map);
        }
        if (IsLeaf())
        {
            for (int x = bottomLeft.x; x < bottomLeft.x + width; x++)
            {
                for (int y = bottomLeft.y; y < bottomLeft.y + height; y++)
                {
                    map[x, y] = (float)DebugId/debugPartitionCounter;
                }
            }
        }
    }
}
