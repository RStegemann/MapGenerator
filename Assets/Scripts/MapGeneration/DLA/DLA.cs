using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to apply DLA algorithms
/// </summary>
public class DLA
{
    struct Walker
    {
        public Walker(Vector2Int p)
        {
            pos = p;
            prevPos = p;
        }

        public Vector2Int pos;
        public Vector2Int prevPos;
    }

    public enum Spawnposition
    {
        Center,
        Edge,
        Center_or_edge,
        Corner,
        Full_Random
    }


    /// <summary>
    /// Takes an already generated map and uses DLA on top of it
    /// </summary>
    /// <param name="map"> Map to edit </param>
    /// <param name="erosionPercent"> Percentage of the eligible tiles to manipulate </param>
    /// <param name="seed"> prng seed </param>
    /// <param name="neighbourHeight"> Minimum elevation at which to apply DLA </param>
    /// <param name="digDepth"> Amount to subtract from the current height on a hit </param>
    public static void PostProcessingDLA(ref float[,] map, float erosionPercent, int seed, float neighbourHeight, float digDepth, Spawnposition pos)
    {
        if (digDepth == 0) return;

        int xSize = map.GetLength(0);
        int ySize = map.GetLength(1);
        int carveAmount = 0;
        System.Random prng = new System.Random(seed);

        bool elevating = digDepth < 0;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (!elevating ? map[x, y] > neighbourHeight : map[x, y] < neighbourHeight) carveAmount++;
            }
        }

        carveAmount = (int)(carveAmount * erosionPercent);
        
        // Spawn Walker on random Position, based on Enum
        Walker w = new Walker(GetRandomPos(pos, xSize, ySize, prng));

        while (carveAmount > 0)
        {
            if (!elevating ? map[w.pos.x, w.pos.y] <= neighbourHeight : map[w.pos.x, w.pos.y] >= neighbourHeight)
            {
                if (!elevating ? map[w.prevPos.x, w.prevPos.y] > neighbourHeight : map[w.prevPos.x, w.prevPos.y] < neighbourHeight)
                {
                    map[w.prevPos.x, w.prevPos.y] = Mathf.Clamp01(map[w.prevPos.x, w.prevPos.y] - digDepth);
                    carveAmount--;
                    w.pos = GetRandomPos(pos, xSize, ySize, prng);
                    continue;
                }
            }
            w.prevPos = w.pos;
            // Move orthogonally
            switch (prng.Next(0, 4))
            {
                // Right
                case 0:
                    w.pos.x += w.pos.x < xSize - 1 ? 1 : 0;
                    break;
                // Left
                case 1:
                    w.pos.x -= w.pos.x > 0 ? 1 : 0;
                    break;
                // Up
                case 2:
                    w.pos.y += w.pos.y < ySize - 1 ? 1 : 0;
                    break;
                // Down
                case 3:
                    w.pos.y -= w.pos.y > 0 ? 1 : 0;
                    break;
            }
        }
    }

    private static Vector2Int GetRandomPos(Spawnposition p, int xSize, int ySize, System.Random prng)
    {
        Vector2Int pos = new Vector2Int();

        switch (p)
        {
            case Spawnposition.Center_or_edge:
                bool center = prng.NextDouble() < 0.2f;
                if (center)
                {
                    goto case Spawnposition.Center;
                }
                else
                {
                    goto case Spawnposition.Edge;
                }
            case Spawnposition.Center:
                pos = new Vector2Int(xSize / 2, ySize / 2);
                break;
            case Spawnposition.Edge:
                pos = prng.NextDouble() > 0.5f ?
                    new Vector2Int(prng.NextDouble() > 0.5f ? 0 : xSize - 1, prng.Next(0, ySize - 1)) :
                    new Vector2Int(prng.Next(0, xSize - 1), prng.NextDouble() > 0.5f ? 0 : ySize - 1);
                break;
            case Spawnposition.Corner:
                pos = new Vector2Int(prng.NextDouble() > 0.5f ? 0 : xSize - 1, prng.NextDouble() > 0.5f ? 0 : ySize - 1);
                break;
            case Spawnposition.Full_Random:
                pos = new Vector2Int(prng.Next(0, xSize - 1), prng.Next(0, ySize - 1));
                break;
        }

        return pos;
    }
}
