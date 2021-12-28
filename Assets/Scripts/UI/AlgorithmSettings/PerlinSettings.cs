using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerlinSettings : MonoBehaviour
{
    public BattleMapSettings settings;

    public void Start()
    {
        SetPerlinLacunarity(0.0001f);
        SetPerlinScale(3);
        SetPerlinPersistence(0.0001f);
        SetPerlinOctaves(1);
    }

    public void SetPerlinScale(float f)
    {
        settings.PerlinScale = f;
    }

    public void SetPerlinPersistence(float f)
    {
        settings.PerlinPersistence = f;
    }

    public void SetPerlinLacunarity(float f)
    {
        settings.PerlinLacunarity = f;
    }

    public void SetPerlinOctaves(float x)
    {
        settings.PerlinOctaves = (int)x;
    }
}
