using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public BattleMapSettings mapData;
    public Tilemap battleMap;
    public Tilemap highlights;
    public Tilemap gridOutlines;

    public Tile highlight;

    private TilemapControls controls;
    private Camera mainCamera;
    private Vector3Int currentTile;

    public TerrainSettings terrainSettings;
    public ObjectSettings objectSettings;
    public PathMenu pathMenu;

    public GameObject spawnPrefab;
    public Tilemap objectLayer;


    private void Awake()
    {
        controls = new TilemapControls();
    }

    private void OnEnable()
    {
        controls.Mouse.Enable();
    }

    private void OnDisable()
    {
        controls.Mouse.Disable();
        currentTile = new Vector3Int();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        controls.Mouse.LeftButton.performed += _ => OnLeftClick();
    }

    public BattleMapGround GetTile(int x, int y)
    {
        return (BattleMapGround)battleMap.GetTile(new Vector3Int(x, y, 0));
    }

    public void SetTile(BattleMapGround tile)
    {
        battleMap.SetTile(tile.pos, tile);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mPos = controls.Mouse.Position.ReadValue<Vector2>();
        Vector3Int tilePos = battleMap.WorldToCell(mainCamera.ScreenToWorldPoint(new Vector3(mPos.x, mPos.y, -mainCamera.transform.position.z)));
        if(tilePos != currentTile && battleMap.HasTile(tilePos))
        {
            highlights.SetTile(currentTile, null);
            highlights.SetTile(tilePos, highlight);
            highlights.RefreshTile(currentTile);
            highlights.RefreshTile(tilePos);
            currentTile = tilePos;
        }
    }

    public void OnLeftClick()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (objectSettings.isActiveAndEnabled && objectSettings.Selected != null)
        {
            Vector2 mPos = controls.Mouse.Position.ReadValue<Vector2>();
            Vector3Int tilePos = battleMap.WorldToCell(mainCamera.ScreenToWorldPoint(new Vector3(mPos.x, mPos.y, -mainCamera.transform.position.z)));
            objectSettings.CreateOrMoveSpawner(new Vector3(0.5f, 0.5f, 0f) + battleMap.CellToWorld(tilePos), objectLayer, battleMap);
        }else if (pathMenu.isActiveAndEnabled && pathMenu.ActivePath != null)
        {
            Vector2 mPos = controls.Mouse.Position.ReadValue<Vector2>();
            Vector3Int tilePos = battleMap.WorldToCell(mainCamera.ScreenToWorldPoint(new Vector3(mPos.x, mPos.y, -mainCamera.transform.position.z)));
            if(battleMap.HasTile(tilePos))pathMenu.ActivePath.AddPoint((BattleMapGround)battleMap.GetTile(tilePos));
        }
    }
}
