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

    //�����ʼ״̬
    public string currentState = "Walk";

    //���齡�����
    public bool isDeath = false;
    public float currentHealth = 0;
    public float maxHealth = 9f;

    //������Ϊ���
    public bool isPursuit = false;
    public float patrolDistance = 6f;

    //���鹥�����
    public float aliveIntervalTime = 3f;
    public float hurtNum = 1f;

    //����״̬���
    public GhostState currentAnim;
    public float curCtuRebirthTime = 0;
}
