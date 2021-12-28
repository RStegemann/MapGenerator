using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapGeneration : MonoBehaviour
{

    public BattleMapSettings mapSettings;
    public TileMapManager manager;
    public BattleMapGround heightTile;
    public Sprite whiteSquare;
    public Sprite gridOutline;
    public float[,] heightMap;
    public CameraControls camCons;

    public void Start()
    {
        Texture2D texOutline = TextureScaling.Resize(gridOutline.texture, mapSettings);
        gridOutline = Sprite.Create(
            texOutline, 
            new Rect(0, 0, mapSettings.pixelsPerField, mapSettings.pixelsPerField), 
            new Vector2(0.5f, 0.5f), 
            Mathf.Min(mapSettings.pixelsPerField, mapSettings.pixelsPerField));

        Texture2D texWhite = TextureScaling.Resize(whiteSquare.texture, mapSettings);
        whiteSquare = Sprite.Create(
            texWhite,
            new Rect(0, 0, mapSettings.pixelsPerField, mapSettings.pixelsPerField),
            new Vector2(0.5f, 0.5f),
            Mathf.Min(mapSettings.pixelsPerField, mapSettings.pixelsPerField));
    }

    /// <summary>
    /// Chooses a BattleMapTile with a certain terrain based on height-values
    /// </summary>
    /// <param name="height"> Height at a specific Tile of the tilemap </param>
    /// <returns> BattleMapTile </returns>
    private Sprite ChooseTerrain(float height)
    {
        if (mapSettings.terrainSettings.Count == 0) return whiteSquare;
        foreach(TerrainData t in mapSettings.terrainSettings)
        {
            if(height >= t.minimumHeight)
            {
                return t.ground;
            }
        }
        return mapSettings.terrainSettings[mapSettings.terrainSettings.Count - 1].ground;
    }

    /// <summary>
    /// Fills map with Tiles to avoid instantiating them whenever the map is drawn
    /// </summary>
    public void FillMap()
    {
        manager.battleMap.ClearAllTiles();
        manager.gridOutlines.ClearAllTiles();
        manager.battleMap.size = new Vector3Int(mapSettings.mapSize.x, mapSettings.mapSize.y, 0);
        manager.gridOutlines.size = new Vector3Int(mapSettings.mapSize.x, mapSettings.mapSize.y, 0);

        for (int x = 0; x < mapSettings.mapSize.x; x++)
        {
            for (int y = 0; y < mapSettings.mapSize.y; y++)
            {
                BattleMapGround tile;
                tile = Instantiate(heightTile);
                tile.sprite = whiteSquare;
                tile.pos = new Vector3Int(x, y, 0);
                manager.SetTile(tile);
                tile.sprite = gridOutline;
                manager.gridOutlines.SetTile(tile.pos, tile);
            }
        }
        camCons.MoveCamToCenter();
    }

    /// <summary>
    /// Generates a map full of float values, to assign terrain pieces to them later on
    /// </summary>
    public void GenerateMap()
    {
        switch (mapSettings.Algorithm)
        {
            case Algorithms.PerlinNoise:
                heightMap = MapGenerator.PerlinNoiseMap(mapSettings.mapSize,
                    mapSettings.PerlinScale,
                    mapSettings.Seed,
                    mapSettings.PerlinPersistence, 
                    mapSettings.PerlinLacunarity, 
                    mapSettings.PerlinOctaves);
                break;
            case Algorithms.SpatialSubdivision:
                heightMap = MapGenerator.SpatialSubdivision(mapSettings.mapSize.x, mapSettings.Seed, mapSettings.SubdivisionPersistence);
                break;
            case Algorithms.BinarySpacePartition:
                heightMap = MapGenerator.BinarySpacePartitionRooms(mapSettings.mapSize, 
                    mapSettings.MinRoomSize, 
                    mapSettings.MaxRoomSize, 
                    mapSettings.Seed, 
                    mapSettings.RoomAmount, 
                    mapSettings.HallwaySize, 
                    mapSettings.RandomizeHallwaySize);
                if (mapSettings.DlaEnabled)
                {
                    DLA.PostProcessingDLA(ref heightMap, mapSettings.ErosionLevel, mapSettings.Seed, mapSettings.MinHeight, mapSettings.DigDepth, mapSettings.DlaSpawn);
                }
                break;
        }

        UpdateMap();
    }

    /// <summary>
    /// Updates the map state without instantiating new Tiles
    /// </summary>
    public void UpdateMap()
    {
        for (int x = 0; x < mapSettings.mapSize.x; x++)
        {
            for (int y = 0; y < mapSettings.mapSize.y; y++)
            {
                float height = heightMap[x, y];
                BattleMapGround tile = manager.GetTile(x, y);

                switch (mapSettings.Output)
                {
                    case Outputs.Battlemap:
                        tile.sprite = ChooseTerrain(height);
                        break;
                    default:
                        tile.sprite = heightTile.sprite;
                        break;
                }
                if (mapSettings.EnableHeightShadow)
                {
                    float roundedHeight = (float)System.Math.Round(heightMap[x, y], 2);
                    tile.color = new Color(roundedHeight, roundedHeight, roundedHeight);
                }
                else
                {
                    tile.color = new Color(1, 1, 1);
                }
                tile.height = height;
            }
        }
        manager.battleMap.RefreshAllTiles();
    }

    public void DeletePath(List<BattleMapGround> groundTiles)
    {
        foreach(BattleMapGround g in groundTiles)
        {
            switch (mapSettings.Output)
            {
                case Outputs.Battlemap:
                    g.sprite = ChooseTerrain(g.height);
                    break;
                default:
                    g.sprite = heightTile.sprite;
                    break;
            }
            manager.battleMap.RefreshTile(g.pos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

