using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static ObjectSettings;

/// <summary>
/// Monobehaviour to handle object panel instances
/// </summary>
public class ObjectPanelSettings : MonoBehaviour
{
    public InputField widthField;
    public InputField heightField;
    public Text objectName;
    public Toggle activeState;
    public Image image;

    public ObjectSettings objectSettings;
    public ObjectData data;
    public List<TerrainData> terrainData;

    private GameObject spawner;

    public InputField spawnWidth;
    public InputField spawnHeight;
    public InputField spawnAmount;
    public InputField seedField;
    public Dropdown targetGround;

    /// <summary>
    /// Initializes Inputfield content types, then generates a seed
    /// </summary>
    public void Start()
    {
        widthField.contentType = InputField.ContentType.DecimalNumber;
        heightField.contentType = InputField.ContentType.DecimalNumber;
        spawnWidth.contentType = InputField.ContentType.IntegerNumber;
        spawnWidth.text = "1";
        spawnHeight.contentType = InputField.ContentType.IntegerNumber;
        spawnHeight.text = "1";
        spawnAmount.contentType = InputField.ContentType.IntegerNumber;
        spawnAmount.text = "1";
        seedField.contentType = InputField.ContentType.IntegerNumber;
        RndSeed();
    }

    /// <summary>
    /// Set terrain data for target ground selection
    /// </summary>
    /// <param name="data"> Terrain data as configured in the terrain settings </param>
    public void SetTerrainData(List<TerrainData> data)
    {
        terrainData = data;
        List<Dropdown.OptionData> groundOptions = new List<Dropdown.OptionData>();
        Dropdown.OptionData o = new Dropdown.OptionData();
        o.text = "None";
        groundOptions.Add(o);
        foreach(TerrainData d in data)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = d.ground.name;
            groundOptions.Add(option);
        }
        targetGround.options = groundOptions;
    }

    /// <summary>
    /// Sets the sprite to spawn with the spawner on this panel.
    /// Reads texture ratio and initializes height/width values accordingly
    /// </summary>
    /// <param name="sprite"></param>
    public void SetObjectTile(Sprite sprite)
    {
        data.ratio = ((float)sprite.texture.width / sprite.texture.height);
        int w;
        int h;
        if (data.ratio > 1)
        {
            w = Mathf.CeilToInt(1f * data.ratio);
            h = 1;
        }
        else if (data.ratio < 1)
        {
            h = Mathf.CeilToInt(1f / data.ratio);
            w = 1;
        }
        else
        {
            h = 1;
            w = 1;
        }

        widthField.text = w.ToString();
        heightField.text = h.ToString();
        data.width = w;
        data.height = h;
        data.image = sprite;

        objectName.text = sprite.name;
        image.sprite = sprite;
    }

    /// <summary>
    /// Sets this panel as the active one, setter in objectSettings clears the previous selection status
    /// </summary>
    /// <param name="b"> true/false </param>
    public void SetActive(bool b)
    {
        objectSettings.Selected = b ? this : null;
    }

    /// <summary>
    /// Sets new height in tiles for the sprite on this object.
    /// Also adjusts the width to keep width/height ratio consistent.
    /// Adjusts Pixels per unit of the texture to achieve smaller/bigger tiles.
    /// </summary>
    /// <param name="s"> String gets parsed to float </param>
    public void SetHeight(string s)
    {
        float old = data.height;
        float oldW = data.width;
        try
        {
            float newHeight = float.Parse(s);
            float mult = newHeight / old;
            float newWidth = oldW * mult;
            widthField.text = (newWidth).ToString();
            Texture2D tex = image.sprite.texture;
            tex = TextureScaling.Resize(tex, objectSettings.mapSettings, newWidth, newHeight);
            data.image = Sprite.Create((Texture2D)tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
            data.height = Mathf.CeilToInt(newHeight);
            data.width = Mathf.CeilToInt(newWidth);
            if(spawner != null) spawner.GetComponent<ObjectSpawner>().Data = data;
        }
        catch
        {
            heightField.text = old.ToString();
            widthField.text = oldW.ToString();
        }
    }

    /// <summary>
    /// Sets new width in tiles for the sprite on this object.
    /// Also adjusts the height to keep width/height ratio consistent.
    /// Adjusts Pixels per unit of the texture to achieve smaller/bigger tiles.
    /// </summary>
    /// <param name="s"> String gets parsed to float </param>
    public void SetWidth(string s)
    {
        float old = data.width;
        float oldH = data.height;
        try
        {
            float newWidth = float.Parse(s);
            float mult = newWidth / old;
            float newHeight = data.height * mult;
            heightField.text = (newHeight).ToString();
            Texture2D tex = image.sprite.texture;
            tex = TextureScaling.Resize(tex, objectSettings.mapSettings, newWidth, newHeight);
            data.image = Sprite.Create((Texture2D)tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
            data.height = Mathf.CeilToInt(newHeight);
            data.width = Mathf.CeilToInt(newWidth);
            if (spawner != null) spawner.GetComponent<ObjectSpawner>().Data = data;
        }
        catch
        {
            widthField.text = old.ToString();
            heightField.text = oldH.ToString();
        }
    }

    /// <summary>
    /// Destroys this panel and the spawner and associated references if existing
    /// </summary>
    public void RemovePanel()
    {
        if (spawner != null)
        {
            spawner.GetComponent<ObjectSpawner>().RemoveSpawnedObjects();
            objectSettings.RemoveSpawnerFromList(spawner);
            Destroy(spawner);
        }
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Sets the amount of objects to spawn
    /// </summary>
    /// <param name="s"> string parsed to int </param>
    public void SetSpawnerAmount(string s)
    {
        if(spawner != null)
        {
            string old = spawnAmount.text;
            try
            {
                spawner.GetComponent<ObjectSpawner>().SpawnAmount = int.Parse(s);
            }
            catch
            {
                spawnAmount.text = old;
            }
        }
    }

    /// <summary>
    /// Sets height of spawn area
    /// </summary>
    /// <param name="s"> string parsed to int </param>
    public void SetSpawnerHeight(string s)
    {
        if (spawner != null)
        {
            string old = spawnHeight.text;
            try
            {
                spawner.GetComponent<ObjectSpawner>().Height = int.Parse(s);
            }
            catch
            {
                spawnHeight.text = old;
            }
        }
    }

    /// <summary>
    /// Sets width of spawn area
    /// </summary>
    /// <param name="s"> string parsed to int </param>
    public void SetSpawnerWidth(string s)
    {
        if (spawner != null)
        {
            string old = spawnWidth.text;
            try
            {
                spawner.GetComponent<ObjectSpawner>().Width = int.Parse(s);
            }
            catch
            {
                spawnWidth.text = old;
            }
        }
    }

    /// <summary>
    /// Target ground on which to spawn objects 
    /// </summary>
    /// <param name="i"> index of the dropdown menu </param>
    public void SetTargetGround(int i)
    {
        if (spawner != null)
        {
            if(i != 0)
            {
                spawner.GetComponent<ObjectSpawner>().TargetGround = terrainData[i - 1];
            }
            else
            {
                spawner.GetComponent<ObjectSpawner>().TargetGround = new TerrainData();
            }
        }
    }

    /// <summary>
    /// Sets seed to use on the spawner
    /// </summary>
    /// <param name="s"> string parsed to int </param>
    public void SetSeed(string s)
    {
        if (spawner != null)
        {
            string old = seedField.text;
            try
            {
                spawner.GetComponent<ObjectSpawner>().Seed = int.Parse(s);
            }
            catch
            {
                seedField.text = old;
            }
        }
    }

    /// <summary>
    /// Generates a random seed and calls the seed setter afterwards
    /// </summary>
    public void RndSeed()
    {
        seedField.text = Guid.NewGuid().GetHashCode().ToString();
        SetSeed(seedField.text);
    }

    /// <summary>
    /// Creates the spawner object for this panel
    /// </summary>
    /// <param name="position"> Center position of the spawner </param>
    /// <param name="objectLayer"> Object layer of the Tilemap </param>
    /// <param name="groundLayer"> Ground layer of the Tilemap </param>
    /// <returns> Spawner gameobject with attaced Objectspawner monobehaviour </returns>
    public GameObject CreateSpawner(Vector3 position, Tilemap objectLayer, Tilemap groundLayer)
    {
        if(spawner == null)
        {
            spawner = Instantiate(objectSettings.objectSpawnerPrefab, position, Quaternion.identity);
            ObjectSpawner spawnSettings = spawner.GetComponent<ObjectSpawner>();
            spawnSettings.Init(position,
                int.Parse(spawnWidth.text),
                int.Parse(spawnHeight.text),
                int.Parse(spawnAmount.text),
                int.Parse(seedField.text),
                targetGround.value != 0 ? terrainData[targetGround.value - 1] : new TerrainData(),
                data,
                objectLayer,
                groundLayer);
        }
        return spawner;
    }

    /// <summary>
    /// Checks if spawner exists
    /// </summary>
    /// <returns> True if spawner exists, otherwise false </returns>
    public bool HasSpawner()
    {
        return spawner != null;
    }

    /// <summary>
    /// Moves the spawner to a different position
    /// </summary>
    /// <param name="position"> Center position of target grid tile </param>
    public void MoveSpawner(Vector3 position)
    {
        spawner.GetComponent<ObjectSpawner>().Position = position;
    }

    public void SetSpawnRenderer(bool b)
    {
        if(spawner != null)
        {
            spawner.GetComponent<SpriteRenderer>().enabled = b;
        }
    }
}
