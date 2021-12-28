using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainPanelSettings : MonoBehaviour
{
    public Text terrainName;
    public Slider heightSlider;
    public Image texture;
    public Toggle activeState;
    private TerrainData terrain;
    public TerrainSettings terrainSettings;
    public InputField sliderValue;

    // Start is called before the first frame update
    void Start()
    {
        sliderValue.contentType = InputField.ContentType.DecimalNumber;
        sliderValue.text = heightSlider.value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGround(TerrainData terrain)
    {
        this.terrain = terrain;
        texture.sprite = terrain.ground;
        terrainName.text = terrain.ground.name;
        heightSlider.value = terrain.minimumHeight;
    }

    public TerrainData GetTerrainData()
    {
        return terrain;
    }

    public void SetHeightFromSlider(float f)
    {
        terrain.minimumHeight = f;
        sliderValue.text = f.ToString();
        terrainSettings.Reorder();
        SettingsMenu.OnSettingsChanged?.Invoke();
    }

    public void SetHeightFromField(string s)
    {
        float old = terrain.minimumHeight;
        try
        {
            terrain.minimumHeight = float.Parse(s);
            heightSlider.value = terrain.minimumHeight;
        }
        catch
        {
            terrain.minimumHeight = old;
            sliderValue.text = old.ToString();
            heightSlider.value = old;
        }

        terrainSettings.Reorder();
        SettingsMenu.OnSettingsChanged?.Invoke();
    }

    public void SetSliderBounds(float min, float max)
    {
        heightSlider.minValue = min;
        heightSlider.maxValue = max;
    }

    public void SetActiveState(bool b)
    {
        if (!b)
        {
            terrainSettings.Deactivate(this.gameObject);
        }
        else
        {
            terrainSettings.Activate(this.gameObject);
        }
        heightSlider.interactable = b;
        sliderValue.interactable = b;
        SettingsMenu.OnSettingsChanged?.Invoke();
    }

    public void RemoveFromList()
    {
        terrainSettings.RemovePanel(this.gameObject, terrain);
    }
}
