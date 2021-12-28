using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public delegate void OnSettingsChangedDelegate();
    public static OnSettingsChangedDelegate OnSettingsChanged;

    public BattleMapSettings settings;
    public TileMapGeneration generator;

    [Header("UI-Components")]
    public Toggle heightshadowToggle;
    public Dropdown algMenu;
    public Dropdown outputMenu;
    public Dropdown menuDropdown;
    public InputField seedInput;
    public InputField heightInput;
    public InputField widthInput;

    [Header("Menus")]
    public GameObject generalParent;
    public GameObject terrainParent;
    public GameObject objectParent;
    public GameObject pathParent;
    public GameObject active;

    [Header("SettingMonos")]
    public TerrainSettings terrainSettings;
    public ObjectSettings objectSettings;

    [Header("AlgorithmSettings")]
    public GameObject perlinSettings;
    public GameObject subdivisionSettings;
    public GameObject BSPSettings;

    public bool autoUpdate;

    // Start is called before the first frame update
    void Start()
    {
        string[] algValues = Enum.GetNames(typeof(Algorithms));
        List<Dropdown.OptionData> algData = new List<Dropdown.OptionData>();
        foreach (string s in algValues)
        {
            Dropdown.OptionData d = new Dropdown.OptionData();
            d.text = s;
            algData.Add(d);
        }

        algMenu.options = algData;

        string[] outputValues = Enum.GetNames(typeof(Outputs));
        List<Dropdown.OptionData> outputData = new List<Dropdown.OptionData>();
        foreach (string s in outputValues)
        {
            Dropdown.OptionData d = new Dropdown.OptionData();
            d.text = s;
            outputData.Add(d);
        }
        outputMenu.options = outputData;

        SetAlgorithm(algMenu.value);
        SetOutput(outputMenu.value);
        SetHeightShadow(heightshadowToggle.isOn);

        seedInput.contentType = InputField.ContentType.IntegerNumber;

        heightInput.text = "60";
        widthInput.text = "60";
        SetHeight("60");
        SetWidth("60");
        SetAutoUpdate(true);
    }

    public void SetAlgorithm(int index)
    {
        settings.Algorithm = (Algorithms)Enum.GetValues(typeof(Algorithms)).GetValue(index);
        switch (settings.Algorithm)
        {
            case Algorithms.PerlinNoise:
                perlinSettings.SetActive(true);
                subdivisionSettings.SetActive(false);
                BSPSettings.SetActive(false);
                break;
            case Algorithms.SpatialSubdivision:
                subdivisionSettings.SetActive(true);
                perlinSettings.SetActive(false);
                BSPSettings.SetActive(false);
                break;
            case Algorithms.BinarySpacePartition:
                BSPSettings.SetActive(true);
                subdivisionSettings.SetActive(false);
                perlinSettings.SetActive(false);
                break;
        }
    }

    public void SetOutput(int index)
    {
        Outputs output = (Outputs)Enum.GetValues(typeof(Outputs)).GetValue(index);
        if (output.Equals(Outputs.Heightmap))
        {
            settings.EnableHeightShadow = true;
            heightshadowToggle.interactable = false;
        }
        else
        {
            heightshadowToggle.interactable = true;
        }
        settings.Output = output;
    }

    public void SetHeightShadow(bool b)
    {
        settings.EnableHeightShadow = b;
    }

    public void SetSeed(string seed)
    {
        int oldValue = settings.Seed;
        try
        {
            settings.Seed = Int32.Parse(seed);
        }
        catch
        {
            settings.Seed = oldValue;
            seedInput.text = oldValue.ToString();
        }
    }

    public void SetWidth(string width)
    {
        int oldValue = settings.Seed;
        try
        {
            settings.mapSize.x = Int32.Parse(width);
        }
        catch
        {
            settings.mapSize.x = oldValue;
            seedInput.text = oldValue.ToString();
        }
        generator.FillMap();
    }

    public void SetHeight(string height)
    {
        int oldValue = settings.Seed;
        try
        {
            settings.mapSize.y = Int32.Parse(height);
        }
        catch
        {
            settings.mapSize.y = oldValue;
            seedInput.text = oldValue.ToString();
        }
        generator.FillMap();
    }

    public void GenerateSeed()
    {
        seedInput.text = Guid.NewGuid().GetHashCode().ToString();
        SetSeed(seedInput.text);
    }

    public void GenerateMap()
    {
        if(seedInput.text == "")
        {
            GenerateSeed();
        }
        settings.terrainSettings = terrainSettings.GetTerrainSettings();
        generator.GenerateMap();
        objectSettings.UpdateSpawners();
    }

    public void ChangeTab(int index)
    {
        active.SetActive(false);
        switch (index)
        {
            case 0:
                active = generalParent;
                break;
            case 1:
                active = terrainParent;
                break;
            case 2:
                active = objectParent;
                break;
            case 3:
                active = pathParent;
                break;
            default:
                break;
        }
        active.SetActive(true);
    }


    public void SetAutoUpdate(bool b)
    {
        autoUpdate = b;
        if (autoUpdate)
        {
            OnSettingsChanged += GenerateMap;
        }
        else
        {
            OnSettingsChanged -= GenerateMap;
        }
    }

    public void CloseApplication()
    {
        Application.Quit();
    }
}
