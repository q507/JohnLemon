using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingData
{
    static int id = 1;
    public int ID = GetID();

    public LightingData()
    {
        id++;
    }

    public static int GetID()
    {
        return id;
    }

    public float posX = 0;
    public float posY = 0;
    public float posZ = 0;

    public float switchOffContinueTime = 15f;
    public float switchOnContinueTime = 10f;

    public float lightingDistance = 5f;

    //×´Ì¬Ïà¹Ø
    public bool isSwitch = false;
    public float curCtuSwitchOffTime = 0;
    public float curCtuSwitchOnTime = 0;
}
