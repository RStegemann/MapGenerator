using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubdivisionSettings : MonoBehaviour
{
    public BattleMapSettings settings;
    public Slider persistenceSlider;

    public void Start()
    {
        persistenceSlider.value = settings.SubdivisionPersistence;
    }

    public void SetSubdivisionPersistence(float f)
    {
        settings.SubdivisionPersistence = f;
    }
}
