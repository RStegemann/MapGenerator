using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;
using UnityEngine.Tilemaps;
using static ObjectSettings;

public class ObjectSpawner : MonoBehaviour
{
    public BattleMapObject objectTile;
    private List<BattleMapObject> spawnedObjects = new List<BattleMapObject>();

    // Private backing fields
    private Vector2 position;
    private int width;
    private int height;
    private int seed;
    private int spawnAmount;
    private TerrainData targetGround;
    private ObjectData data;

    private System.Random prng;

    public Tilemap objectLayer;
    public Tilemap groundLayer;

    public void Start()
    {
        gameObject.transform.localScale = new Vector3(width, height, 1);
    }

    public Vector2 Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
            gameObject.transform.position = position;
            Spawn();
        }
    }

    public int Width
    {
        get
        {
            return width;
        }
        set
        {
            width = value;
            this.gameObject.transform.localScale = new Vector3(width, gameObject.transform.localScale.y, 1);
            Spawn();
        }
    }

    public int Height
    {
        get
        {
            return height;
        }
        set
        {
            height = value;
            this.gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, height, 1);
            Spawn();
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
            prng = new System.Random(seed);
            Spawn();
        }
    }

    public int SpawnAmount
    {
        get
        {
            return spawnAmount;
        }
        set
        {
            spawnAmount = value;
            Spawn();
        }
    }

    public TerrainData TargetGround
    {
        get
        {
            return targetGround;
        }
        set
        {
            targetGround = value;
            Spawn();
        }
    }

    public ObjectData Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
            Spawn();
        }
    }

    public void Spawn()
    {
        RemoveSpawnedObjects();
        RandomSpawns();
    }

    public void RemoveSpawnedObjects()
    {
        if (spawnedObjects.Count > 0)
        {
            foreach (BattleMapObject o in spawnedObjects)
            {
                objectLayer.SetTile(new Vector3Int(o.pos.x, o.pos.y, 0), null);
            }
        }
        spawnedObjects.Clear();
    }

    public void Init(Vector2 pos, int w, int h, int c, int s, TerrainData t, ObjectData d, Tilemap oL, Tilemap gL)
    {
        objectLayer = oL;
        groundLayer = gL;
        position = pos;
        width = w;
        height = h;
        spawnAmount = c;
        targetGround = t;
        data = d;
        Seed = s; // Also generates prng and calls Spawn
    }

    /// <summary>
    /// Same as RandomGroundBasedSpawns, except without checking for target ground matching.
    /// In a different function to avoid additional conditions within loops.
    /// </summary>
    public void RandomSpawns()
    {
        int objW = data.width;
        int objH = data.height;
        List<Vector3Int> positions = GetValidPositions(objW, objH);

        int count = spawnAmount;
        while (count > 0 && positions.Count > 0)
        {
            int index = prng.Next(0, positions.Count - 1);
            Vector3Int p = positions[index];
            bool valid = CheckSpreadPosition(p.x, p.y, objW, objH);
            if (!valid)
            {
                positions.RemoveAt(index);
                continue;
            }

            BattleMapObject o = Instantiate(objectTile);
            o.pos = (Vector2Int)p;
            o.sprite = data.image;
            o.width = objW;
            o.height = objH;

            objectLayer.SetTile(p, o);
            positions.RemoveAt(index);
            spawnedObjects.Add(o);

            Matrix4x4 m = objectLayer.GetTransformMatrix(p);
            m.SetTRS(new Vector3(0.5f * (objW - 1), 0.5f * (objH - 1), 0), Quaternion.identity, Vector3.one);
            objectLayer.SetTransformMatrix(p, m);
            count--;


            List<Vector3Int> spawnPositions = new List<Vector3Int>();
            for (int i = 0; i < o.width; i++)
            {
                for (int j = 0; j < o.height; j++)
                {
                    if (i == 0 && j == 0) continue;
                    spawnPositions.Add(new Vector3Int(p.x + i, p.y + j, 0));
                }
            }

            foreach (Vector3Int pp in spawnPositions)
            {
                BattleMapObject oo = Instantiate(objectTile);
                oo.pos = (Vector2Int)pp;
                oo.width = o.width;
                oo.height = o.height;
                objectLayer.SetTile(pp, oo);
                spawnedObjects.Add(oo);
            }
        }
    }


    /// <summary>
    /// Determines valid positions for an object of a given size.
    /// If targetground is set, calls GetValidGroundPositions() 
    /// </summary>
    /// <param name="w"> Grid width of the object </param>
    /// <param name="h"> Grid height of the object </param>
    /// <returns> List of valid positions </returns>
    public List<Vector3Int> GetValidPositions(int w, int h)
    {
        List<Vector3Int> positions = new List<Vector3Int>();
        int xStart = CeilToInt(position.x - (width / 2f));
        int xWidth = FloorToInt(position.x + width / 2f- (w - 1));
        int yStart = CeilToInt(position.y - (height / 2f));
        int yWidth = FloorToInt(position.y + height / 2f - (h - 1));
        for (int x = xStart; x < xWidth; x++)
        {
            for (int y = yStart; y < yWidth; y++)
            {
                if(CheckSpreadPosition(x, y, w, h))
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    positions.Add(pos);
                }
            }
        }
        return positions;
    }

    /// <summary>
    /// Checks if an object of width objW and height objH, fits in the spot (x,y)
    /// </summary>
    /// <param name="x">x coordinate on tilemap</param>
    /// <param name="y">y coordinate on tilemap</param>
    /// <param name="objW">Width of the object</param>
    /// <param name="objH">Height of the object</param>
    /// <returns></returns>
    private bool CheckSpreadPosition(int x, int y, int objW, int objH)
    {
        for (int px = x; px < x + objW; px++)
        {
            for (int py = y; py < y + objH; py++)
            {
                Vector3Int spreadPos = new Vector3Int(px, py, 0);
                if (!groundLayer.HasTile(spreadPos) || 
                    objectLayer.HasTile(spreadPos) ||
                    (targetGround.ground != null && !((BattleMapGround)groundLayer.GetTile(spreadPos)).sprite.Equals(targetGround.ground)))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
