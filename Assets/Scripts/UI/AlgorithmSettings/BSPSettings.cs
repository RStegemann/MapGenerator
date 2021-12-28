using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BSPSettings : MonoBehaviour
{
    public InputField minRoomSize;
    public InputField maxRoomSize;
    public InputField roomAmount;
    public InputField hallwaySize;
    public Toggle randomizeHallwaySize;

    public BattleMapSettings settings;

    public void Start()
    {
        randomizeHallwaySize.isOn = settings.RandomizeHallwaySize;

        minRoomSize.contentType = InputField.ContentType.IntegerNumber;
        maxRoomSize.contentType = InputField.ContentType.IntegerNumber;
        roomAmount.contentType = InputField.ContentType.IntegerNumber;
        hallwaySize.contentType = InputField.ContentType.IntegerNumber;
    }

    public void SetMinRoomSize(string s)
    {
        int oldValue = settings.MinRoomSize;
        try
        {
            int n = Int32.Parse(s);
            if(n > settings.MaxRoomSize || n == 1)
            {
                minRoomSize.text = oldValue.ToString();
                return;
            }
            settings.MinRoomSize = n;
        }
        catch
        {
            minRoomSize.text = oldValue.ToString();
            settings.MinRoomSize = oldValue;
        }
    }

    public void SetMaxRoomSize(string s)
    {
        int oldValue = settings.MaxRoomSize;
        try
        {
            int n = Int32.Parse(s);
            if(n < settings.MinRoomSize)
            {
                maxRoomSize.text = oldValue.ToString();
                return;
            }
            settings.MaxRoomSize = n;
        }
        catch
        {
            maxRoomSize.text = oldValue.ToString();
            settings.MaxRoomSize = oldValue;
        }
    }

    public void SetRoomAmount(string s)
    {
        int oldValue = settings.RoomAmount;
        try
        {
            settings.RoomAmount = Int32.Parse(s);
        }
        catch
        {
            roomAmount.text = oldValue.ToString();
            settings.RoomAmount = oldValue;
        }
    }

    public void SetHallWaySize(string s)
    {
        int oldValue = settings.HallwaySize;
        try
        {
            settings.HallwaySize = Int32.Parse(s);
        }
        catch
        {
            hallwaySize.text = oldValue.ToString();
            settings.HallwaySize = oldValue;
        }
    }

    public void SetRandomHallwaySize(bool b)
    {
        settings.RandomizeHallwaySize = b;
    }
}
