using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DLA;

public class DLASettings : MonoBehaviour
{
    public Toggle dlaEnabled;
    public Dropdown spawnPicker;
    public Slider erosionSlider;
    public Slider minHeightSlider;
    public Slider digDepthSlider;

    public BattleMapSettings settings;

    public void Start()
    {
        List<Dropdown.OptionData> spawnData = new List<Dropdown.OptionData>();
        foreach (string s in Enum.GetNames(typeof(Spawnposition)))
        {
            Dropdown.OptionData d = new Dropdown.OptionData();
            d.text = s;
            spawnData.Add(d);
        }
        spawnPicker.options = spawnData;

        erosionSlider.value = settings.ErosionLevel;
        minHeightSlider.value = settings.MinHeight;
        digDepthSlider.value = settings.DigDepth;
    }

    public void SetSpawnPos(int i)
    {
        settings.DlaSpawn = (Spawnposition)Enum.GetValues(typeof(Spawnposition)).GetValue(i);
    }

    public void SetDLAEnabled(bool b)
    {
        settings.DlaEnabled = b;
    }

    public void SetErosionLevel(float f)
    {
        settings.ErosionLevel = f;
    }

    public void SetDLAMinHeight(float f)
    {
        settings.MinHeight = f;
    }

    public void SetHeightChange(float f)
    {
        settings.DigDepth = f;
    }
}
