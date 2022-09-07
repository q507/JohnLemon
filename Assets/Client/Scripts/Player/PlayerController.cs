using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerAnim
{
    Hit,
    Death,
    Idle,
    Fight,
    BoxOpen,
    SwitchOn,
    SwitchOff,
    Walk,
    Fight_Die,
    Fight_Hit,
    Fight_Idle,
    Fight_Attack,
    Fight_BoxOpen,
    Fight_SwitchOn,
    Fight_SwitchOff,
    Fight_Walk
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] Rigidbody rigidBody;
    public Animator animator;
    [SerializeField] AudioManager audioManager;
    [SerializeField] TouchRocker touchRocker;
    [SerializeField] GameManager gameManager;
    [SerializeField] HealthManager healthManager;
    [SerializeField] GameObject shootPoint;
    [SerializeField] PlayerProjectileSpawner projectile;
    [SerializeField] PlayerProjectile playerProjectile;
    [SerializeField] TreasureChest treasureChest;

    //角色服务端相关数据
    public SceneData sceneData;
    DecelerationTrapData decelerationData;

    private Vector3 moveMent = Vector3.zero;
    //角色键盘输入移动相关参数
    //private float rotateSpeed = 150.0f;
    private float gravity = 10.0f;

    //角色能量相关
    public float maxEnergy = 100f;
    public float currentEnergy = 0;
    private float addEnergy = 1.5f;
    private float reduceEnergy = 3.0f;

    private float currentHorizontalValue = 0;
    private float currentVerticalValue = 0;

    private float moveTime = 0;
    private float moveInterval = 1.0f;
    private bool isHurt = false;
    public bool isSlowDown = false;

    private float invincibleTime = 0;
    private float shootTime = 0;
    private float tempTime = 0;

    private void Start()
    {
        sceneData.playData = new PlayerData();
        sceneData.playData.playerAnim = PlayerAnim.Idle;

        decelerationData = new DecelerationTrapData();

        gameManager.PlayerDeathEvent += PlayerDeath;
        healthManager.onDie += PlayerDeath;
        treasureChest.OpenChest += PlayerOpenChest;

        currentEnergy = maxEnergy;
    }

    void Update()
    {
        if (isHurt)
        {
            //与服务端进行强同步
            tempTime = Time.deltaTime;
            sceneData.playData.curCtuBeatTime = tempTime;
            invincibleTime += sceneData.playData.curCtuBeatTime;
            if (invincibleTime > 1)
            {
                invincibleTime = 0;
            }
        }
        else
        {
            invincibleTime = 0;
        }
        shootTime += Time.deltaTime;

        //角色停止移动时停止播放脚步声
        if (currentHorizontalValue == 0 && currentVerticalValue == 0)
        {
            sceneData.playData.isMoving = false;

            EventManager.Send(audioManager);
        }

        //获取当前键盘移动的输入值
        currentHorizontalValue = Input.GetAxis("Horizontal");
        currentVerticalValue = Input.GetAxis("Vertical");

        //获取当前摇杆滑动传入的值
        if (!isSlowDown)
        {
            sceneData.playData.touchHorizontal = touchRocker.horizontal;
            sceneData.playData.touchVertical = touchRocker.vertical;
            animator.speed = sceneData.playData.moveSpeed;
        }
        else
        {
            sceneData.playData.touchHorizontal = touchRocker.horizontal * decelerationData.slowMultiple;
            sceneData.playData.touchVertical = touchRocker.vertical * decelerationData.slowMultiple;
            animator.speed = decelerationData.slowMultiple;
        }

        _CharacterMoveEvent();
        _PlayerMovement();
        _PlayerShoot();
        _PlayAudio();

        //能量自然增长
        StartCoroutine(AddEnergy());

        UIManager.Instance.UpdateEnergy(currentEnergy / maxEnergy);
    }

    //玩家移动
    private void _CharacterMoveEvent()
    {
        if (sceneData.playData.isDeath)
        {
            return;
        }

        sceneData.playData.isMoving = true;

        //TODO：按键切换操作角色移动方式
        if (characterController.isGrounded)
        {
            //键盘输入移动相关
            /*_moveMent = new Vector3(0, 0, _currentVerticalValue * _moveSpeed * Time.deltaTime);
            _moveMent = transform.TransformDirection(_moveMent);*/

            //摇杆移动相关
            moveMent = new Vector3(sceneData.playData.touchVertical * Time.deltaTime * Time.deltaTime, 0, sceneData.playData.touchHorizontal * Time.deltaTime * Time.deltaTime);

            //加速
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.speed = 2.0f;
                StartCoroutine(ReduceEnergy());
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                animator.speed = 1.0f;
            }
        }
        moveMent.y -= gravity * Time.deltaTime;
        characterController.Move(moveMent);

        //transform.Rotate(0, _currentHorizontalValue * _rotateSpeed * Time.deltaTime, 0);
    }

    //动画事件
    private void _PlayerMovement()
    {
        if (!sceneData.playData.isWeapon)
        {
            //摇杆移动相关
            if (sceneData.playData.touchHorizontal != 0 || sceneData.playData.touchVertical != 0)
            {
                animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Idle).ToString(), false);
                animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Walk).ToString(), true);
            }
            else
            {
                animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Walk).ToString(), false);
                animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Idle).ToString(), true);
            }
        }
        else if (sceneData.playData.isWeapon)
        {
            //角色持有武器
            if (sceneData.playData.touchHorizontal != 0 || sceneData.playData.touchVertical != 0)
            {
                animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Fight_Idle).ToString(), false);
                animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Fight_Walk).ToString(), true);
            }
            else
            {
                animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Fight_Walk).ToString(), false);
                animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Fight_Idle).ToString(), true);
            }
        }
    }

    private void _PlayerShoot()
    {
        if (sceneData.playData.isWeapon)
        {
            if(shootTime - sceneData.playData.shootIntervalTime > 0)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Fight_Attack).ToString(), true);

                    //对象池生成子弹
                    projectile.SpawnProjectile();
                    shootTime = 0;
                }
                else
                {
                    animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Fight_Attack).ToString(), false);
                }
            }
        }
    }

    public void PlayerOpenChest()
    {
        animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Idle).ToString(), false);
        animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.BoxOpen).ToString(), true);

        sceneData.playData.isWeapon = true;
        animator.SetBool("IsWeapon", true);
    }

    public void PlayerDeath()
    {
        if (!sceneData.playData.isDeath)
        {
            if (!sceneData.playData.isWeapon)
            {
                sceneData.playData.isDeath = true;
                animator.SetTrigger((sceneData.playData.playerAnim = PlayerAnim.Death).ToString());
                animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Idle).ToString(), false);
            }
            else
            {
                sceneData.playData.isDeath = true;
                animator.SetTrigger((sceneData.playData.playerAnim = PlayerAnim.Fight_Die).ToString());
                animator.SetBool((sceneData.playData.playerAnim = PlayerAnim.Idle).ToString(), false);
            }
        }
    }
    
    //音效
    private void _PlayAudio()
    {
        if (sceneData.playData.isMoving)
        {
            moveTime += Time.deltaTime;

            if(moveTime >= moveInterval)
            {
                moveTime = 0;

                EventManager.Send(audioManager);
            }
        }
    }

    //摇杆相关
    public void RotatePlayer(float angle)
    {
        if (sceneData.playData.isDeath)
        {
            return;
        }

        Vector3 tmpAngle = transform.localEulerAngles;
        tmpAngle.y = angle;
        transform.localEulerAngles = tmpAngle;
    }

    //受伤
    public void Hurt(float damage)
    {
        isHurt = true;
        int randomBehaviour = Random.Range(1, 2);

        //受伤，开始无敌
        healthManager.TakeDamage(damage);
        if (!sceneData.playData.isWeapon)
        {
            animator.SetTrigger((sceneData.playData.playerAnim = PlayerAnim.Hit).ToString() + randomBehaviour);
        }
        else
        {
            animator.SetTrigger((sceneData.playData.playerAnim = PlayerAnim.Fight_Hit).ToString() + randomBehaviour);
        }
        sceneData.playData.invincible = true;

        if (invincibleTime > 0 && invincibleTime - sceneData.playData.invincibleIntervalTime < 0)
        {
            isHurt = false;
            //无敌时间结束
            sceneData.playData.invincible = false;
        }
    }

    IEnumerator AddEnergy()
    {
        while (currentEnergy <= maxEnergy)
        {
            currentEnergy += addEnergy;
            if(currentEnergy >= maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
            yield return null;
        }
    }

    IEnumerator ReduceEnergy()
    {
        while (currentEnergy > 0)
        {
            currentEnergy -= reduceEnergy;
            if(currentEnergy <= 0)
            {
                currentEnergy = 0;
            }
            yield return null;
        }
    }
}
