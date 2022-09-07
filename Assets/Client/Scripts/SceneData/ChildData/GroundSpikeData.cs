using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpikeData
{
    static int id = 1;
    public int ID = GetID();

    public GroundSpikeData()
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

    public float intervalTime = 3f;
    public float continueTime = 3f;
    public float hurtIntervalTime = 1f;
    public float damage = 1f;

    //×´Ì¬Ïà¹Ø
    public bool isUp = false;
    public float curCtuIntervalTime = 0;
    public float curCtuContinueTime = 0;
}
