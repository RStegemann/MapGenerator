using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSettings : MonoBehaviour
{
    private readonly string FOLDER = "Assets/Textures";

    public BattleMapSettings mapSettings;
    public GameObject objectPanelPrefab;
    public GameObject contentHolder;
    public BattleMapObject battleMapObjectPrefab;
    public GameObject objectSpawnerPrefab;

    private ObjectPanelSettings selected;
    private List<GameObject> spawners = new List<GameObject>();

    /// <summary>
    /// Struct to contain all the relevant data for a gameobject
    /// </summary>
    public struct ObjectData
    {
        public int width;
        public int height;
        public float ratio;
        public Sprite image;
    }

    /// <summary>
    /// Clears current selection status and sets it to the newly selected sprite
    /// </summary>
    public ObjectPanelSettings Selected
    {
        get
        {
            return selected;
        }
        set
        {
            if (selected != null)
            {
                selected.SetSpawnRenderer(false);
                selected.activeState.isOn = false;
            }

            selected = value;

            if (selected != null)
            {
                selected.SetSpawnRenderer(true);
            }
        }
    }

    /// <summary>
    /// Instantiates an object panel and initializes it for the given sprite.
    /// </summary>
    /// <param name="sprite"></param>
    public void CreateObjectPanel(Sprite sprite)
    {
        GameObject prefab = Instantiate(objectPanelPrefab, contentHolder.transform);
        prefab.GetComponent<ObjectPanelSettings>().SetObjectTile(sprite);
        prefab.GetComponent<ObjectPanelSettings>().objectSettings = this;
        if(mapSettings.terrainSettings != null && mapSettings.terrainSettings.Count > 0) prefab.GetComponent<ObjectPanelSettings>().SetTerrainData(mapSettings.terrainSettings);
    }

    /// <summary>
    /// Opens a File selection window in which one or multiple texture files can be selected.
    /// Creates one object panel for every selected texture.
    /// </summary>
    public void OpenTexturePicker()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Pick Textures", FOLDER, new ExtensionFilter[] { new ExtensionFilter("Image Files", "jpg", "png") }, true);

        foreach (string path in paths)
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            float mult = tex.width > tex.height ? tex.width / tex.height : tex.height / tex.width;
            tex = TextureScaling.Resize(tex, mapSettings, tex.width > tex.height ? mult : 1, tex.width > tex.height ? 1 : mult);
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), Mathf.Min(tex.width, tex.height));
            sprite.name = Path.GetFileName(path);
            CreateObjectPanel(sprite);
        }
    }

    /// <summary>
    /// Cause all spawners to delete their current spawns and spawn new objects
    /// </summary>
    public void UpdateSpawners()
    {
        foreach(GameObject spawner in spawners)
        {
            spawner.GetComponent<ObjectSpawner>().Spawn();
        }
    }

    /// <summary>
    /// Creates a spawner for the currently active panel if it doesn't have one yet.
    /// If a spawner for the current panel exists already, move it to the click position
    /// </summary>
    /// <param name="position"> Center point of the clicked grid tile </param>
    /// <param name="objectLayer"> Object layer of the Tilemap </param>
    /// <param name="groundLayer"> Ground layer of the tilemap </param>
    public void CreateOrMoveSpawner(Vector3 position, Tilemap objectLayer, Tilemap groundLayer)
    {
        if (Selected.HasSpawner())
        {
            Selected.MoveSpawner(position);
        }
        else
        {
            spawners.Add(Selected.CreateSpawner(position, objectLayer, groundLayer));
        }
    }

    /// <summary>
    /// Removes a destroyed spawner from the list
    /// </summary>
    /// <param name="o"> Spawner gameobject to remove </param>
    public void RemoveSpawnerFromList(GameObject o)
    {
        spawners.Remove(o);
    }
}
