using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class BattleMapGround : Tile
{
    public Vector3Int pos;
    public string terrainName;
    public float height;
}
