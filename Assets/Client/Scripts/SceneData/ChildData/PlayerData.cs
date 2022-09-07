using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    //摇杆相关参数
    public float touchHorizontal = 0;
    public float touchVertical = 0;

    //角色初始位置
    public float playerPosX = -17.73f;
    public float playerPosY = 0;
    public float playerPosZ = -0.98f;

    //角色移动相关
    public bool isMoving = false;
    public float moveSpeed = 1.0f;

    //角色健康相关
    public bool isDeath = false;
    public float currentHealth = 0;
    public float maxHealth = 3;
    public bool invincible = false;
    public float invincibleIntervalTime = 1.0f;

    //角色武器相关
    public bool isWeapon = false;
    public float shootIntervalTime = 0.5f;

    //角色状态相关
    //public int animation = 0;
    public PlayerAnim playerAnim;
    public float curCtuBeatTime = 0;
}
