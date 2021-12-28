using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TerrainSettings : MonoBehaviour
{
    private readonly string FOLDER = "Assets/Textures";

    public BattleMapSettings mapSettings;
    private List<TerrainData> groundTiles = new List<TerrainData>();
    private List<GameObject> activePanels = new List<GameObject>();
    private List<GameObject> inactivePanels = new List<GameObject>();
    public GameObject terrainPanelPrefab;
    public GameObject contentHolder;

    public void Awake()
    {

    }

    public void Start()
    {
    }

    public void Activate(GameObject o)
    {
        inactivePanels.Remove(o);
        activePanels.Add(o);
        mapSettings.terrainSettings.Add(o.GetComponent<TerrainPanelSettings>().GetTerrainData());
    }

    public void Deactivate(GameObject o)
    {
        activePanels.Remove(o);
        inactivePanels.Add(o);
        mapSettings.terrainSettings.Remove(o.GetComponent<TerrainPanelSettings>().GetTerrainData());
    }

    public void Reorder()
    {

    }

    private void AddTerrainPanels()
    {
        foreach (TerrainData g in groundTiles)
        {
            CreateTerrainPanel(g);
        }
    }

    private void CreateTerrainPanel(TerrainData d)
    {
        GameObject prefab = Instantiate(terrainPanelPrefab, contentHolder.transform);
        prefab.GetComponent<TerrainPanelSettings>().SetGround(d);
        inactivePanels.Add(prefab);
        prefab.GetComponent<TerrainPanelSettings>().terrainSettings = this;
    }

    public List<TerrainData> GetTerrainSettings()
    {
        List<TerrainData> data = new List<TerrainData>();
        foreach(GameObject o in activePanels)
        {
            data.Add(o.GetComponent<TerrainPanelSettings>().GetTerrainData());
        }
        data.Sort((h1,h2) => h2.minimumHeight.CompareTo(h1.minimumHeight));

        return data;
    }

    public void OpenTexturePicker()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Pick Textures", FOLDER, new ExtensionFilter[] { new ExtensionFilter("Image Files", "jpg", "png") }, true);

        foreach(string path in paths)
        {
            TerrainData d = new TerrainData();
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            tex = TextureScaling.Resize(tex, mapSettings);
            d.ground = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), tex.width);
            d.ground.name = Path.GetFileName(path);
            groundTiles.Add(d);
            CreateTerrainPanel(d);
        }
    }

    public void RemovePanel(GameObject o, TerrainData d)
    {
        if (activePanels.Contains(o))
        {
            activePanels.Remove(o);
        }
        else
        {
            inactivePanels.Remove(o);
        }
        groundTiles.Remove(d);
        Destroy(o);
        SettingsMenu.OnSettingsChanged?.Invoke();
    }

}
