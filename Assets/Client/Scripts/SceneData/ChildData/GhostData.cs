using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostData
{
    static int id = 1;
    public int ID = GetID();

    public GhostData()
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

    //幽灵初始状态
    public string currentState = "Walk";

    //幽灵健康相关
    public bool isDeath = false;
    public float currentHealth = 0;
    public float maxHealth = 9f;

    //幽灵行为相关
    public bool isPursuit = false;
    public float patrolDistance = 6f;

    //幽灵攻击相关
    public float aliveIntervalTime = 3f;
    public float hurtNum = 1f;

    //幽灵状态相关
    public GhostState currentAnim;
    public float curCtuRebirthTime = 0;
}
