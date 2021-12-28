using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DLA;

[CreateAssetMenu]
public class BattleMapSettings : ScriptableObject
{
    // Public variables which don't have autoupdate getter/setter methods
    public List<TerrainData> terrainSettings;
    public Vector2Int mapSize;

    // Used in texture resizing
    public int pixelsPerField;

    // Private backing fields for properties for autoupdate
    private Algorithms algorithm;
    private Outputs output;
    private Spawnposition spawnPosition;
    private bool enableHeightShadow, randomizeHallwaySize, dlaEnabled;
    private int seed, perlinOctaves, minRoomSize, maxRoomsize, roomAmount, hallwaySize;
    private float perlinScale, perlinLacunarity, perlinPersistence, subdivisionPersistence, erosionLevel, minHeight, digDepth;

    public void OnEnable()
    {
        terrainSettings = new List<TerrainData>();
    }

    public Algorithms Algorithm
    {
        get
        {
            return algorithm;
        }
        set
        {
            algorithm = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public Outputs Output
    {
        get
        {
            return output;
        }
        set
        {
            output = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }
    public bool EnableHeightShadow
    {
        get
        {
            return enableHeightShadow;
        }
        set
        {
            enableHeightShadow = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }
    public int Seed
    {
        get
        {
            return seed;
        }
        set
        {
            seed = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public float PerlinScale
    {
        get
        {
            return perlinScale;
        }
        set
        {
            perlinScale = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public float PerlinPersistence
    {
        get
        {
            return perlinPersistence;
        }
        set
        {
            perlinPersistence = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public float PerlinLacunarity
    {
        get
        {
            return perlinLacunarity;
        }
        set
        {
            perlinLacunarity = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public int PerlinOctaves
    {
        get
        {
            return perlinOctaves;
        }
        set
        {
            perlinOctaves = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public float SubdivisionPersistence
    {
        get
        {
            return subdivisionPersistence;
        }

        set
        {
            subdivisionPersistence = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public int MinRoomSize
    {
        get
        {
            return minRoomSize;
        }
        set
        {
            minRoomSize = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public int MaxRoomSize
    {
        get
        {
            return maxRoomsize;
        }
        set
        {
            maxRoomsize = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public int RoomAmount
    {
        get
        {
            return roomAmount;
        }
        set
        {
            roomAmount = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public int HallwaySize
    {
        get
        {
            return hallwaySize;
        }
        set
        {
            hallwaySize = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public bool RandomizeHallwaySize
    {
        get
        {
            return randomizeHallwaySize;
        }
        set
        {
            randomizeHallwaySize = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public bool DlaEnabled
    {
        get
        {
            return dlaEnabled;
        }
        set
        {
            dlaEnabled = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public float ErosionLevel
    {
        get
        {
            return erosionLevel;
        }
        set
        {
            erosionLevel = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public float MinHeight
    {
        get
        {
            return minHeight;
        }
        set
        {
            minHeight = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public float DigDepth
    {
        get
        {
            return digDepth;
        }
        set
        {
            digDepth = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }

    public Spawnposition DlaSpawn
    {
        get
        {
            return spawnPosition;
        }
        set
        {
            spawnPosition = value;
            SettingsMenu.OnSettingsChanged?.Invoke();
        }
    }
}
