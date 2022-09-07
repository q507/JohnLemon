using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    //ҡ����ز���
    public float touchHorizontal = 0;
    public float touchVertical = 0;

    //��ɫ��ʼλ��
    public float playerPosX = -17.73f;
    public float playerPosY = 0;
    public float playerPosZ = -0.98f;

    //��ɫ�ƶ����
    public bool isMoving = false;
    public float moveSpeed = 1.0f;

    //��ɫ�������
    public bool isDeath = false;
    public float currentHealth = 0;
    public float maxHealth = 3;
    public bool invincible = false;
    public float invincibleIntervalTime = 1.0f;

    //��ɫ�������
    public bool isWeapon = false;
    public float shootIntervalTime = 0.5f;

    //��ɫ״̬���
    //public int animation = 0;
    public PlayerAnim playerAnim;
    public float curCtuBeatTime = 0;
}
