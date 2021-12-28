using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathMenu : MonoBehaviour
{
    private PathSettings activePath;

    public GameObject container;
    public GameObject pathPanelPrefab;
    public TileMapGeneration generator;

    public PathSettings ActivePath
    {
        get
        {
            return activePath;
        }
        set
        {
            if(activePath != null) activePath.active.isOn = false;
            activePath = value;
        }
    }

    public void AddPathPanel()
    {
        GameObject o = Instantiate(pathPanelPrefab, container.transform);
        o.GetComponent<PathSettings>().pathMenu = this;
        o.GetComponent<PathSettings>().mapGenerator = generator;
    }

    public void AddPointToPath(BattleMapGround pos)
    {
        activePath.AddPoint(pos);
    }
}
