using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class PoissonDiscSampling
{
    public static List<Vector2Int> GeneratePoints(int minDistance, Vector2Int sampleArea, int numTries, int seed, Tilemap objects, Tilemap ground, BattleMapGround targetGround)
    {
        System.Random prng = new System.Random(seed);

        float cellSize = minDistance / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleArea.x / cellSize), Mathf.CeilToInt(sampleArea.y / cellSize)];
        List<Vector2Int> points = new List<Vector2Int>();
        List<Vector2Int> spawnPoints = new List<Vector2Int>();

        spawnPoints.Add(sampleArea / 2);
        while(spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2Int spawnCenter = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for(int i = 0; i < numTries; i++)
            {
                float angle = (float)prng.NextDouble() * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 worldPos = spawnCenter + dir * (float)(prng.NextDouble() * minDistance + minDistance);
                Vector2Int candidate = new Vector2Int((int)worldPos.x, (int)worldPos.y);
                if (IsValid(candidate, objects, ground, targetGround))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }
        return points;
    }

    private static bool IsValid(Vector2Int candidate, Tilemap objects, Tilemap ground, BattleMapGround targetGround)
    {
        bool valid = false;
        return valid;
    }
}
