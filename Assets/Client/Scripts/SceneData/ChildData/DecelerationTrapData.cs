using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecelerationTrapData
{
    static int id = 1;
    public int ID = GetID();

    public DecelerationTrapData()
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

    public float slowMultiple = 0.5f;
    public float slowContinueTime = 5f;

    //×´Ì¬Ïà¹Ø
    public bool isSlowDown = false;
    public float curCtuSlowTime = 0;
}
